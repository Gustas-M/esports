using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using esports.Data;
using esports.Models;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Transactions;

namespace esports.Controllers
{
    [Route("Teams")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    //[ApiController]
    public class TeamsController : Controller
    {
        private readonly EsportsContext _context;

        public TeamsController(EsportsContext context)
        {
            _context = context;
        }

        // GET: Championships
        /// <summary>
        /// Returns a list of all championships
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Teams.ToListAsync());
        }

        // GET: Championships/Details/5
        /// <summary>
        /// Returns a specific championship
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        /// <summary>
        /// Creates a new championship with specified parameters
        /// </summary>
        /// <param name="championship"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] TeamDto team)
        {
            var tmp = _context.Teams.FirstOrDefault(c => c.Name == team.Name);
            if (tmp != null)
            {
                return UnprocessableEntity();
            }

            if (ModelState.IsValid)
            {
                if (team.IsValid())
                {
                    Team TeamDbObj = new Team(team.Name, HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub));
                    _context.Add(TeamDbObj);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("POST", new { id = TeamDbObj.Id }, TeamDbObj);
                }

                return UnprocessableEntity();

            }

            var validation = new ValidationProblemDetails(ModelState);

            return BadRequest(validation);
        }

        /// <summary>
        /// Edits a championship with specified parameters
        /// </summary>
        /// <param name="id"></param>
        /// <param name="championship"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [FromBody] TeamDto team)
        {
            var teamDbObj = await _context.Teams.FindAsync(id);
            if (teamDbObj == null)
            {
                return NotFound();
            }

            if (!HttpContext.User.IsInRole(Roles.Admin) && HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub) != teamDbObj.UserId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                if (team.IsValid())
                {
                    teamDbObj.Name = team.Name;                    
                    _context.Update(teamDbObj);
                    await _context.SaveChangesAsync();
                    return Ok(team);
                }

                return UnprocessableEntity();
            }

            var validation = new ValidationProblemDetails(ModelState);

            return BadRequest(validation);
        }

        /// <summary>
        /// Deletes a championship with specified ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }



            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            if (!HttpContext.User.IsInRole(Roles.Admin) && HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub) != team.UserId)
            {
                return Forbid();
            }


            _context.Teams.Remove(team);

            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
