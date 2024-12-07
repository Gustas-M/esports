
using esports.Models;
using Microsoft.AspNetCore.Identity;
using System.Transactions;

namespace esports.Data
{
    public class AuthSeeder
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthSeeder(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task SeedAsync()
        {
            await AddDefaultRolesAsync();
            await AddAdminUserAsync();
        }


        private async Task AddAdminUserAsync()
        {
            var newAdminUser = new User
            {
                UserName = "admin",
                Email = "admin@gmail.com"
            };

            var existAdminUser = await _userManager.FindByNameAsync(newAdminUser.UserName);

            if (existAdminUser == null)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var createAdminUserResult = await _userManager.CreateAsync(newAdminUser, "VerySafeP@ss1");
                    if (createAdminUserResult.Succeeded)
                    {
                        var addToRoleResult = await _userManager.AddToRoleAsync(newAdminUser, Roles.Admin);
                        if (addToRoleResult.Succeeded)
                        {
                            transaction.Complete();
                        }
                    }
                }
            }
        }

        private async Task AddDefaultRolesAsync()
        {
            foreach(var role in Roles.AllRoles)
            {
                var existRole = await _roleManager.RoleExistsAsync(role);
                if (!existRole)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
