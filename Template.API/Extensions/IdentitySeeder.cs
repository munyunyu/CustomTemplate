using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Template.Library.Constants;
using Template.Library.Extensions;
using Template.Library.Tables.User;

namespace Template.API.Extensions
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(this IServiceProvider services)
        {
            try
            {
                using var scope = services.CreateScope();

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                foreach (var role in new SystemRoles().GetConstantValues())
                {
                    //seed roles
                    if (!await roleManager.RoleExistsAsync(role)) await roleManager.CreateAsync(new IdentityRole(role));

                    //seed claims
                    await SeedRoleClaims(roleManager, role);

                    //seed roles & claims to admin_user
                    await SeedAdminUser(userManager, roleManager);
                }
            }
            catch (Exception)
            {
                //do nothing
            }
            
        }

        private static async Task SeedRoleClaims(RoleManager<IdentityRole> roleManager, string role)
        {
            var role_details = await roleManager.FindByNameAsync(role);

            if (role_details != null) await EnsureClaims(role_details, new SystemClaims().GetConstantValues(role), roleManager);

        }

        private static async Task EnsureClaims(IdentityRole role, IEnumerable<string> claims, RoleManager<IdentityRole> roleManager)
        {
            var existingClaims = await roleManager.GetClaimsAsync(role);

            foreach (var claim in claims)
            {
                if (!existingClaims.Any(c => c.Type == claim)) await roleManager.AddClaimAsync(role, new Claim(claim, claim));
            }
        }

        private static async Task SeedAdminUser(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminUser = await userManager.FindByIdAsync(SystemUserAdmin.Admin_UserId);

            if (adminUser == null) return;

            // Assign all roles
            foreach (var role in new SystemRoles().GetConstantValues())
            {
                if (!await userManager.IsInRoleAsync(adminUser, role)) await userManager.AddToRoleAsync(adminUser, role);
            }

            // Assign all claims
            var userClaims = await userManager.GetClaimsAsync(adminUser);

            foreach (var claim in new SystemClaims().GetConstantValues())
            {
                if (!userClaims.Any(c => c.Type == claim)) await userManager.AddClaimAsync(adminUser, new Claim(claim, claim));
            }
        }
    }

}
