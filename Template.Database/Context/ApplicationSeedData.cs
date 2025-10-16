using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Database.Metadata;
using Template.Library.Tables.Notification;

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

        public void SeedEmailConfigs()
        {
            var data = DatabaseMetadata.GetEmailConfigs();

            builder.Entity<TblEmailConfig>().HasData(data);
        }

        public void SeedEmailTemplat()
        {
            var data = DatabaseMetadata.GetEmailTemplat();

            builder.Entity<TblEmailTemplat>().HasData(data);
        }


    }
}
