using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Database.Metadata;

namespace Template.Database.Context
{
    public class ApplicationSeedData
    {
        private readonly ModelBuilder builder;

        public ApplicationSeedData(ModelBuilder builder)
        {
            this.builder = builder;
        }

        public void SeedRoles()
        {
            var roles = DatabaseMetadata.GetSeedRoles();

            builder.Entity<IdentityRole>().HasData(roles);
        }

        public void SeedUsers()
        {
            var users = DatabaseMetadata.GetSeedUsers();

            builder.Entity<ApplicationUser>().HasData(users);
        }

        public void SeedUserRoles()
        {
            var userRoles = DatabaseMetadata.GetUserRoles();

            builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        }

        

       
    }
}
