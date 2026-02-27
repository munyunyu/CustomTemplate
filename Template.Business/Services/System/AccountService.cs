using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Template.Business.Interfaces.System;
using Template.Database.Context;
using Template.Library.Constants;
using Template.Library.Exceptions;
using Template.Library.Extensions;
using Template.Library.Models;
using Template.Library.Tables.Audit;
using Template.Library.Tables.User;
using Template.Library.ViewsModels.System;

namespace Template.Business.Services.System
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDatabaseService database;
        private readonly IMapper mapper;
        public AccountService(UserManager<ApplicationUser> userManager, IDatabaseService database, IMapper mapper)
        {
            this.userManager = userManager;
            this.database = database;
            this.mapper = mapper;
        }

        public async Task<string> AddClaimToUser(ApplicationUser user, string claimType)
        {
            var exits = new SystemClaims().GetConstantValues().Any(x =>  x == claimType);

            if (exits == false) throw new GeneralException($"Claim: {claimType} was not found");
            
            var claims = await userManager.GetClaimsAsync(user);

            // Check if the user has the specified claim
            if (claims.Any(c => c.Type == claimType)) throw new GeneralException($"Claim: {claimType} already exists");

            var claim = new Claim(claimType, claimType);

            var result = await userManager.AddClaimAsync(user, claim);

            if (result.Succeeded) return "Success";

            throw new GeneralException($"Failed to add claim: {claimType}");
        }

        public async Task<string> RemoveClaimToUser(ApplicationUser user, string? claimType)
        {
            var exits = new SystemClaims().GetConstantValues().Any(x => x == claimType);

            if (exits == false) throw new GeneralException($"Claim: {claimType}of this type is not registered");

            var claims = await userManager.GetClaimsAsync(user);

            // Check if the user has the specified claim
            if (claims.Any(c => c.Type == claimType) == false) throw new GeneralException($"Claim: {claimType} was not found for this user");

            var claim = new List<Claim> { new Claim(claimType ?? "", claimType ?? "") };

            var result = await userManager.RemoveClaimsAsync(user, claim);

            if (result.Succeeded) return "Success";

            throw new GeneralException($"Failed to remove claim: {claimType}");
        }

        public async Task<string> AddRoleToUserAsync(ApplicationUser user, string role)
        {
            var exits = new SystemRoles().GetConstantValues().Any(x => x == role);

            if (exits == false) throw new GeneralException($"Role: {role} was not found");

            exits = await userManager.IsInRoleAsync(user, role);

            if (exits) throw new GeneralException($"Role: {role} already exists");

            var result = await userManager.AddToRoleAsync(user, role);

            if (result.Succeeded) return "Success";

            throw new GeneralException($"Failed to add role: {role}");
        }

        public async Task<string> RemoveRoleToUserAsync(ApplicationUser user, string role)
        {
            var exits = new SystemRoles().GetConstantValues().Any(x => x == role);

            if (exits == false) throw new GeneralException($"Role: {role} was not found");

            exits = await userManager.IsInRoleAsync(user, role);

            if (exits == false) throw new GeneralException($"Role: {role} was not found for this user");

            var result = await userManager.RemoveFromRoleAsync(user, role);

            if (result.Succeeded) return "Success";

            throw new GeneralException($"Failed to remove role: {role}");
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string? password, bool requireConfirmedEmail)
        {
            if (string.IsNullOrEmpty(password)) throw new Exception("password is required");

            if(requireConfirmedEmail && user.EmailConfirmed == false) throw new Exception($"Email is not confirmed");

            var isvalid = await userManager.CheckPasswordAsync(user, password);

            if(isvalid == false) return false;

            await database.AddAsync(new TblAuditLog
            {
                Id = Guid.NewGuid(),
                Action = "Login",
                Changes = user.Email ?? string.Empty,
                EntityId = Guid.Parse(user.Id),
                EntityName = "ApplicationUser",
                CreatedById = Guid.Parse(user.Id),
                CreatedDate = DateTime.UtcNow,
                LastUpdatedById = Guid.Parse(user.Id),
                LastUpdatedDate = DateTime.UtcNow,
            });

            return isvalid;
        }

        public async Task<IdentityResult> CreateAccountAsync(ApplicationUser user, string password)
        {
            var result = await userManager.CreateAsync(user, password);

            return result;
        }

        public async Task<IdentityResult> UpdateAccountAsync(ApplicationUser user)
        {
            var response = await userManager.UpdateAsync(user);

            return response;
        }

        public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string oldPassword, string newPassword)
        {
            var response = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            return response;
        }

        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string newPassword)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            if (string.IsNullOrEmpty(newPassword)) throw new GeneralException("Failed to generate reset token");      

            var response = await userManager.ResetPasswordAsync(user, token, newPassword);

            return response;
        }

        public async Task<ApplicationUser?> FindByIdAsync(Guid userId)
        {
            ApplicationUser? user = await userManager.FindByIdAsync(userId.ToString());

            //if (user == null) throw new Exception($"User with userId: {userId} was not found");

            return user;
        }

        public async Task<ApplicationUser?> FindByNameAsync(string? email)
        {
            if (string.IsNullOrEmpty(email)) throw new Exception("Email is  required");

            ApplicationUser? user = await userManager.FindByNameAsync(email);

            return user;
        }

        public async Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user)
        {
            var claims = await userManager.GetClaimsAsync(user);

            return claims.ToList();
        }

        public async Task<ResponseUserRolesAndClaims> GetUserRolesAndClaimsAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null) throw new Exception($"User Id: {userId} was not found");

            var roles = (await userManager.GetRolesAsync(user)).ToList();

            var claims = (await userManager.GetClaimsAsync(user)).Select(x => x.Type).ToList();

            return new ResponseUserRolesAndClaims { Claims = claims, Roles = roles };
        }

        public async Task<List<string>> GetUserRolesAsync(ApplicationUser user)
        {
            var roles = await userManager.GetRolesAsync(user);

            return roles.ToList();
        }

        public async Task<ApplicationUser> FindByProfileIdAsync(Guid profileId)
        {
            var profile = await database.GetAsync<TblProfile>(x => x.Id == profileId);

            if (profile == null) throw new GeneralException($"Profile with Id: {profileId} was not found");

            ApplicationUser? user = await userManager.FindByIdAsync(profile.UserId.ToString());

            return user ?? throw new GeneralException($"User Id: {profile.Id} with this profile Id: {profileId} was not found");
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            var result = await userManager.GenerateEmailConfirmationTokenAsync(user);

            return result;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            var result = await userManager.ConfirmEmailAsync(user, token);

            return result;
        }

        public async Task<bool> IsPasswordExpiredAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return false;

            // If password never expires, return false
            if (user.PasswordNeverExpires) return false;

            // Check if password has expired (90 days)
            if (user.LastPasswordChangeDate.HasValue)
            {
                var expiryDate = user.LastPasswordChangeDate.Value.AddDays(90);
                return DateTime.UtcNow > expiryDate;
            }

            // If no last change date, consider it expired
            return true;
        }

        public async Task<bool> CheckPasswordHistoryAsync(string userId, string newPassword)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var previousPasswords = await database.GetAllAsync<TblPasswordHistory>(x => x.UserId == userId, count: 5);

            foreach (var oldPassword in previousPasswords)
            {
                var result = userManager.PasswordHasher.VerifyHashedPassword(user, oldPassword.PasswordHash!, newPassword);
                if (result == PasswordVerificationResult.Success)
                {
                    return false; // Password found in history
                }
            }

            return true; // Password not in history
        }

        public async Task HandleFailedLoginAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return;

            user.FailedLoginAttempts++;

            if (user.FailedLoginAttempts >= 3)
            {
                user.LockoutEndDate = DateTime.UtcNow.AddMinutes(5); // Lock for 5 minutes
                user.FailedLoginAttempts = 0; // Reset counter
            }

            await userManager.UpdateAsync(user);
        }

        public async Task ResetFailedLoginAttemptsAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return;

            user.FailedLoginAttempts = 0;
            await userManager.UpdateAsync(user);
        }

        public async Task<bool> IsAccountLockedAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return true;

            // Check if manually locked
            if (user.IsManuallyLocked) return true;

            // Check if lockout period has ended
            if (user.LockoutEndDate.HasValue && user.LockoutEndDate > DateTime.UtcNow)
            {
                return true;
            }

            // If lockout period has ended, reset it
            if (user.LockoutEndDate.HasValue && user.LockoutEndDate <= DateTime.UtcNow)
            {
                user.LockoutEndDate = null;
                await userManager.UpdateAsync(user);
            }

            return false;
        }

        public async Task<ApplicationUserViewModel?> GetUserDetailsAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null) throw new Exception($"User Id: {userId} was not found");

            var roles = (await userManager.GetRolesAsync(user)).ToList();

            var claims = (await userManager.GetClaimsAsync(user)).Select(x => x.Type).ToList();

            var lastLogin = await database.GetLastAsync<TblAuditLog>(x => x.EntityName == "ApplicationUser" && x.Action == "Login" && x.EntityId == userId, o => o.CreatedDate);

            var _mapped = mapper.Map<ApplicationUserViewModel>(user);

            _mapped.Roles = roles;
            _mapped.Claims = claims;
            _mapped.LastLogin = lastLogin?.LastUpdatedDate;

            return _mapped;
        }
    }
}
