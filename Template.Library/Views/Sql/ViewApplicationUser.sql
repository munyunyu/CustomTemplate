CREATE OR ALTER VIEW [dbo].[ViewApplicationUser]
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


