using Microsoft.AspNetCore.Identity;
using Template.Database.Context;
using Template.Library.Constants;
using Template.Library.Tables.Notification;

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

        public static TblEmailConfig[] GetEmailConfigs()
        {
            var configs = new TblEmailConfig[]
            {
                new TblEmailConfig()
                {               
                    Name = EmailConfig.Default,
                    SmtpServer = "smtp.gmail.com",
                    SmtpPort = 587,
                    SmtpUser = "user@example.com",
                    SmtpPassword = "password",
                    SmtpEnableSsl = true,

                    Id = Guid.Parse("e7b52cbe-f96a-471d-9b8e-e5fd5c9f3c13"),
                    CreatedById = Guid.Parse(admin_userId),
                    CreatedDate = date,
                    LastUpdatedDate = date
                }
            };

            return configs;
        }

        public static TblEmailTemplat[] GetEmailTemplat()
        {
            var templates = new TblEmailTemplat[]
            {
                new TblEmailTemplat()
                {
                    Id = Guid.Parse("a2ab8f49-8beb-4d80-a42b-2e5629d71a8e"),
                    CreatedById = Guid.Parse(admin_userId),
                    CreatedDate = date,
                    LastUpdatedDate = date,                    
                    
                    Name = EmailTemplat.ConfirmEmail,
                    Subject = "Confirm Email",
                    Body = @"<!DOCTYPE html>
                            <html>
                            
                            <head>
                                <meta charset=""UTF-8"">
                                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                                <title>Confirm Your Email Address</title>
                            </head>
                            
                            <body style=""margin: 0; padding: 0; font-family: Arial, Helvetica, sans-serif; background-color: #f5f5f5;"">
                                <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""border-collapse: collapse;"">
                                    <tr>
                                        <td align=""center"" style=""padding: 40px 0;"">
                                            <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%""
                                                style=""max-width: 600px; border-collapse: collapse; background-color: #ffffff; border-radius: 8px; box-shadow: 0 4px 6px rgba(0,0,0,0.1);"">
                                                <!-- Header -->
                                                <tr>
                                                    <td
                                                        style=""padding: 30px 30px 20px; text-align: center; background-color: #4a6cf7; border-radius: 8px 8px 0 0;"">
                                                        <h1 style=""margin: 0; color: white; font-size: 24px;"">Confirm Your Email Address</h1>
                                                    </td>
                                                </tr>
                            
                                                <!-- Content -->
                                                <tr>
                                                    <td style=""padding: 30px;"">
                                                        <p style=""margin: 0 0 20px; color: #333333; line-height: 1.6;"">Hello [First Name],</p>
                                                        <p style=""margin: 0 0 20px; color: #333333; line-height: 1.6;"">Thank you for signing up for
                                                            [ProductServiceName]. Please confirm that <strong>[EmailAddress]</strong> is your
                                                            email address by clicking the button below:</p>
                            
                                                        <!-- Button -->
                                                        <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%""
                                                            style=""margin: 30px 0;"">
                                                            <tr>
                                                                <td align=""center"">
                                                                    <a href=""[ConfirmationLink]""
                                                                        style=""background-color: #4a6cf7; color: white; text-decoration: none; padding: 12px 24px; border-radius: 4px; display: inline-block; font-weight: bold;"">Confirm
                                                                        Email Address</a>
                                                                </td>
                                                            </tr>
                                                        </table>
                            
                                                        <p style=""margin: 0 0 20px; color: #666666; line-height: 1.6; font-size: 14px;"">If you did
                                                            not create an account with us, please ignore this email.</p>
                            
                                                        <p style=""margin: 0 0 10px; color: #333333; line-height: 1.6;"">Thanks,<br>The [CompanyName] Team
                                                        </p>
                                                    </td>
                                                </tr>
                            
                                                <!-- Footer -->
                                                <tr>
                                                    <td
                                                        style=""padding: 20px 30px; background-color: #f8f9fa; border-radius: 0 0 8px 8px; text-align: center;"">
                                                        <p style=""margin: 0 0 10px; color: #666666; font-size: 12px;"">&copy; 2023 [CompanyName].
                                                            All rights reserved.</p>
                                                        <p style=""margin: 0 0 10px; color: #666666; font-size: 12px;"">[CompanyAddress]</p>
                                                        <p style=""margin: 0; color: #666666; font-size: 12px;"">
                                                            <a href=""#"" style=""color: #4a6cf7; text-decoration: none;"">Unsubscribe</a> |
                                                            <a href=""#"" style=""color: #4a6cf7; text-decoration: none;"">Privacy Policy</a> |
                                                            <a href=""#"" style=""color: #4a6cf7; text-decoration: none;"">Help Center</a>
                                                        </p>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </body>
                            
                            </html>"
                },
                new TblEmailTemplat()
                {
                    Id = Guid.Parse("21bcb9c2-1485-4b09-8691-9fcac34613a4"),
                    CreatedById = Guid.Parse(admin_userId),
                    CreatedDate = date,
                    LastUpdatedDate = date,

                    Name = EmailTemplat.ResetPassword,
                    Subject = "Reset Password",
                    Body = @"<!DOCTYPE html>
                                <html lang=""en"">
                                <head>
                                    <meta charset=""UTF-8"">
                                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                                    <title>Password Reset Request</title>
                                    <style>
                                        body {
                                            margin: 0;
                                            padding: 0;
                                            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                                            background-color: #f5f5f5;
                                            color: #333333;
                                        }
                                        .email-container {
                                            max-width: 600px;
                                            margin: 0 auto;
                                            background-color: #ffffff;
                                        }
                                        .email-header {
                                            background-color: #4a6cf7;
                                            padding: 30px;
                                            text-align: center;
                                            border-radius: 8px 8px 0 0;
                                        }
                                        .email-body {
                                            padding: 30px;
                                        }
                                        .email-footer {
                                            background-color: #f8f9fa;
                                            padding: 20px;
                                            text-align: center;
                                            border-radius: 0 0 8px 8px;
                                            font-size: 12px;
                                            color: #666666;
                                        }
                                        .button {
                                            display: inline-block;
                                            padding: 12px 24px;
                                            background-color: #4a6cf7;
                                            color: white;
                                            text-decoration: none;
                                            border-radius: 4px;
                                            font-weight: bold;
                                            margin: 20px 0;
                                        }
                                        .text-center {
                                            text-align: center;
                                        }
                                        .divider {
                                            border-top: 1px solid #eaeaea;
                                            margin: 25px 0;
                                        }
                                        .logo {
                                            color: white;
                                            font-size: 24px;
                                            font-weight: bold;
                                            text-decoration: none;
                                        }
                                    </style>
                                </head>
                                <body>
                                    <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                                        <tr>
                                            <td align=""center"" style=""padding: 40px 0;"">
                                                <!-- Email Container -->
                                                <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" class=""email-container"">
                                                    <!-- Header -->
                                                    <tr>
                                                        <td class=""email-header"">
                                                            <a href=""#"" class=""logo"">[CompanyName]</a>
                                                        </td>
                                                    </tr>
                                                    
                                                    <!-- Body -->
                                                    <tr>
                                                        <td class=""email-body"">
                                                            <h2 style=""margin-top: 0;"">Reset Your Password</h2>
                                                            <p>Hello [Username],</p>
                                                            <p>We received a request to reset your password for your [UserEmail] account. Click the button below to create a new password:</p>
                                                            
                                                            <div class=""text-center"">
                                                                <a href=""[ResetPasswordLink]"" class=""button"">Reset Password</a>
                                                            </div>
                                                            
                                                            <p>If you didn't request a password reset, please ignore this email. Your password will remain unchanged.</p>
                                                            
                                                            <div class=""divider""></div>
                                                            
                                                            <p style=""margin-bottom: 0;""><strong>Having trouble?</strong> Copy and paste the following link into your browser:</p>
                                                            <p style=""word-break: break-all; color: #4a6cf7; margin-top: 5px;"">[ResetPasswordLink]</p>
                                                        </td>
                                                    </tr>
                                                    
                                                    <!-- Footer -->
                                                    <tr>
                                                        <td class=""email-footer"">
                                                            <p>This email was sent to [UserEmail]. If you didn't request a password reset, <a href=""#"" style=""color: #4a6cf7;"">let us know</a>.</p>
                                                            <p>&copy; 2023 [CompanyName]. All rights reserved.</p>
                                                            <p>[CompanyName], [CompanyAddress]</p>
                                                            <p>
                                                                <a href=""#"" style=""color: #4a6cf7; text-decoration: none;"">Unsubscribe</a> | 
                                                                <a href=""#"" style=""color: #4a6cf7; text-decoration: none;"">Privacy Policy</a> | 
                                                                <a href=""#"" style=""color: #4a6cf7; text-decoration: none;"">Help Center</a>
                                                            </p>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </body>
                                </html>"
                },
            };

            return templates;
        }
    }
}
