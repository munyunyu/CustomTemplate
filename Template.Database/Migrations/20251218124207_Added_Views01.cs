using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Database.Migrations
{
    /// <inheritdoc />
    public partial class Added_Views01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            IF OBJECT_ID('dbo.ViewApplicationUserRoles', 'V') IS NOT NULL
            DROP VIEW dbo.ViewApplicationUserRoles;
            ");

            migrationBuilder.Sql(@"
            CREATE VIEW [dbo].[ViewApplicationUserRoles]
            AS
            SELECT TOP (1000) 
                   [UserId][Id]
                  ,[RoleId]
            	  ,U.Email
            	  ,R.[Name]
            	  ,[CreatedDate]
                  ,[CreatedById]
                  ,[LastUpdatedDate]
                  ,[LastUpdatedById]
                  ,[IsDeleted]
              FROM [TemplateDB].[dbo].[AspNetUserRoles]UR
              LEFT JOIN [TemplateDB].[dbo].[AspNetUsers]U ON U.[Id] = UR.[UserId]
              LEFT JOIN [TemplateDB].[dbo].[AspNetRoles]R ON R.[Id] = UR.[RoleId]
            ");

            migrationBuilder.Sql(@"
            IF OBJECT_ID('dbo.ViewApplicationUser', 'V') IS NOT NULL
            DROP VIEW dbo.ViewApplicationUser;
            ");

            migrationBuilder.Sql(@"
            CREATE VIEW [dbo].[ViewApplicationUser]
            AS
            SELECT  
                 u.[Id],
                 u.[FirstName],
                 u.[LastName],
                 u.[CreatedDate],
                 u.[LastUpdatedDate],
                 u.[IsDeleted],
                 u.[UserName],
                 u.[NormalizedUserName],
                 u.[Email],
                 u.[NormalizedEmail],
                 u.[EmailConfirmed],
                 u.[PasswordHash],
                 u.[SecurityStamp],
                 u.[ConcurrencyStamp],
                 u.[PhoneNumber],
                 u.[PhoneNumberConfirmed],
                 u.[TwoFactorEnabled],
                 u.[LockoutEnd],
                 u.[LockoutEnabled],
                 u.[AccessFailedCount],
                 u.[CreatedById],
                 u.[LastUpdatedById],
                 u.[Description],
                 u.[Hash],
                 
                 -- 👇 Last Login Date
                 MAX(a.CreatedDate) AS LastLogin

            FROM [TemplateDB].[dbo].[AspNetUsers] u
            LEFT JOIN [TemplateDB].[dbo].[TblAuditLog] a
                ON a.EntityId = u.Id
                AND a.EntityName = 'ApplicationUser'
                AND a.[Action] = 'Login'
            
            GROUP BY
                u.[Id],
                u.[FirstName],
                u.[LastName],
                u.[CreatedDate],
                u.[LastUpdatedDate],
                u.[IsDeleted],
                u.[UserName],
                u.[NormalizedUserName],
                u.[Email],
                u.[NormalizedEmail],
                u.[EmailConfirmed],
                u.[PasswordHash],
                u.[SecurityStamp],
                u.[ConcurrencyStamp],
                u.[PhoneNumber],
                u.[PhoneNumberConfirmed],
                u.[TwoFactorEnabled],
                u.[LockoutEnd],
                u.[LockoutEnabled],
                u.[AccessFailedCount],
                u.[CreatedById],
                u.[LastUpdatedById],
                u.[Description],
                u.[Hash];
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS [dbo].[ViewApplicationUserRoles];");
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS [dbo].[ViewApplicationUser];");
        }
    }
}
