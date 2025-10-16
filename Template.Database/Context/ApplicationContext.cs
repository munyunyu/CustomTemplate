using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text;
using Template.Library.Tables;
using Template.Library.Tables.Audit;
using Template.Library.Tables.Notification;
using Template.Library.Tables.User;
using Template.Library.Tables.Views;
using Template.Library.Views.System;

namespace Template.Database.Context
{
    public partial class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ApplicationSeedData data = new ApplicationSeedData(modelBuilder);

            //Seed applicatio roles
            data.SeedRoles();

            //Seed application users
            data.SeedUsers();

            //Seed application user roles
            data.SeedUserRoles();

            //Seed email configs and template
            data.SeedEmailConfigs();

            data.SeedEmailTemplat();

            modelBuilder.Entity<ViewSystemUserRoles>().HasNoKey().ToView(nameof(ViewSystemUserRoles));

            modelBuilder.Entity<ViewApplicationUser>().HasNoKey().ToView(nameof(ViewApplicationUser));


        }


        public virtual DbSet<TblAuditLog>? TblAuditLog { get; set; }
        public virtual DbSet<TblPasswordHistory>? TblPasswordHistory { get; set; }

        public virtual DbSet<TblProfile>? TblProfile { get; set; }

        //Notification
        public virtual DbSet<TblNotification>? TblNotification { get; set; }
        public virtual DbSet<TblEmailQueue>? TblEmailQueue { get; set; }
        public virtual DbSet<TblEmailTemplat>? TblEmailTemplat { get; set; }
        public virtual DbSet<TblEmailConfig>? TblEmailConfig { get; set; }



        #region Views

        public virtual DbSet<ViewSystemUserRoles>? ViewSystemUserRoles { get; set; }
        public virtual DbSet<ViewApplicationUser> ViewApplicationUser { get; set; }

        #endregion



        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var modifiedEntities = ChangeTracker.Entries().Where(e => e.Entity is IAuditable && (e.State == EntityState.Modified || e.State == EntityState.Added || e.State == EntityState.Deleted)).ToList();

            foreach (var modifiedEntity in modifiedEntities)
            {
                var entity = (BaseEntity)modifiedEntity.Entity;

                var auditLog = new TblAuditLog
                {
                    Id = Guid.NewGuid(),
                    EntityId = entity.Id,
                    EntityName = modifiedEntity.Entity.GetType().Name,
                    Action = modifiedEntity.State.ToString(),
                    Changes = GetChanges(modifiedEntity),
                    CreatedById = entity.LastUpdatedById,
                    LastUpdatedById = entity.LastUpdatedById,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now,   
                    Hash = entity.GenerateHash(),
                };

                TblAuditLog?.Add(auditLog);
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private string GetChanges(EntityEntry modifiedEntity)
        {
            var changes = new StringBuilder();

            foreach (var property in modifiedEntity.OriginalValues.Properties)
            {
                var originalValue = modifiedEntity.OriginalValues[property];

                var currentValue = modifiedEntity.CurrentValues[property];

                if (!Equals(originalValue, currentValue)) changes.AppendLine($"{property.Name}: From '{originalValue}' to '{currentValue}'");
            }

            return changes.ToString();
        }
    }
}
