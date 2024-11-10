using esports.Data;
using esports.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace esports.Controllers
{
    public class AuthController : Controller
    {
        private readonly EsportsContext _context;
        private readonly UserManager<User> _userManager;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(EsportsContext context, UserManager<User> userManager, JwtTokenService jwtTokenService)
        {
            _context = context;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }
        /// <summary>
        /// Data transfer object for registering a new user.
        /// </summary>
        public record RegisterUserDto(string UserName, string Email, string Password);
        public record LoginUserDto(string UserName, string Password);
        public record SuccessfulLoginDto(string accessToken);
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userManager">The user manager to handle user operations.</param>
        /// <param name="dto">The data transfer object containing user registration details.</param>
        /// <returns>An ActionResult indicating the result of the registration process.</returns>
        [HttpPost("api/register")]
        public async Task<ActionResult> Register([FromBody]RegisterUserDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user != null)
            {
                return UnprocessableEntity("User already exists");
            }

            var newUser = new User
            {
                UserName = dto.UserName,
                Email = dto.Email
            };

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createUserResult = await _userManager.CreateAsync(newUser, dto.Password);

                if (!createUserResult.Succeeded)
                {
                    return UnprocessableEntity(createUserResult.Errors);
                }

                var addToRoleResult = await _userManager.AddToRoleAsync(newUser, Roles.TeamMember);

                if (!addToRoleResult.Succeeded)
                {
                    return UnprocessableEntity(addToRoleResult.Errors);
                }

                transaction.Complete();
            }

            return Created();
        }

        [HttpPost("api/login")]
        public async Task<ActionResult> Login([FromBody]LoginUserDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
            {
                return UnprocessableEntity("User does not exist");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (isPasswordValid == false)
            {
                return UnprocessableEntity("Username or password was incorrect");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);



            return Ok(new SuccessfulLoginDto(accessToken));
        }

        
    }
}
