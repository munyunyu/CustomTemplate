using Microsoft.AspNetCore.Identity;
using Template.Database.Context;
using Template.Library.Constants;

namespace Template.Database.Metadata
{
    public static class DatabaseMetadata
    {
        const string admin_userId = "feba6c0a-e24c-4410-a8c2-0145bd3d1853";

        static DateTime date = DateTime.Parse("2025-08-11");

        /// <summary>
        /// Genarate system roles
        /// </summary>
        /// <returns></returns>
        public static List<IdentityRole> GetSeedRoles()
        {
            List<IdentityRole> roles = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Id = "4b6e2892-da63-48d5-8ebc-7a3a5a6b9f9a",
                    Name = SystemRoles.Admin,
                    NormalizedName = SystemRoles.Admin.ToUpper(),
                    ConcurrencyStamp = "934eeaf5-0909-4f24-9625-4d31f2332f3a"
                }
            };

            return roles;
        }

        /// <summary>
        /// Genarate system users
        /// </summary>
        /// <returns></returns>
        public static List<ApplicationUser> GetSeedUsers()
        {
            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();

            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser(){Id = admin_userId, Email = "percy.munyunyu@gmail.com", EmailConfirmed = true, FirstName = "admin", LastName = "admin", UserName = "percy.munyunyu@gmail.com", NormalizedUserName = "percy.munyunyu@gmail.com".ToUpper() }
            };

            foreach (var user in users)
            {
                user.PasswordHash = ph.HashPassword(user, "tc#Prog219!");
            }

            return users;
        }


        /// <summary>
        /// Link system users and roles
        /// </summary>
        /// <returns></returns>
        public static List<IdentityUserRole<string>> GetUserRoles()
        {
            var roles = GetSeedRoles();

            List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();

            foreach (var role in roles) userRoles.Add(new IdentityUserRole<string> { UserId = admin_userId, RoleId = role.Id });

            return userRoles;
        }

    }
}
