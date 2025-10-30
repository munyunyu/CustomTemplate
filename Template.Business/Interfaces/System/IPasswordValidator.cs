using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Database.Context;
using Template.Library.Tables.User;

namespace Template.Business.Interfaces.System
{
    public interface IPasswordValidator
    {
        
    }

    public class CustomPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : ApplicationUser
    {
        private readonly ApplicationContext _context;

        public CustomPasswordValidator(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            var result = await Task.FromResult(IdentityResult.Success);

            // Check if password matches any of the previous passwords
            var previousPasswords = await _context.TblPasswordHistory
                .Where(ph => ph.UserId == user.Id)
                .OrderByDescending(ph => ph.CreatedDate)
                .Take(5) // Check against last 5 passwords
                .ToListAsync();

            foreach (var oldPassword in previousPasswords)
            {
                var passwordVerificationResult = manager.PasswordHasher.VerifyHashedPassword(user, oldPassword.PasswordHash, password);
                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "PasswordReuse",
                        Description = "You cannot use a password that you have used before."
                    });
                }
            }

            return result;
        }
    }
}
