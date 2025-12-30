using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Template.Database.Context;
using Template.Library.Models;
using Template.Library.Tables.User;
using Template.Library.ViewsModels.System;

namespace Template.Business.Interfaces.System
{
    public interface IAccountService
    {
        Task<string> AddClaimToUser(ApplicationUser user, string claim);
        Task<string> AddRoleToUserAsync(ApplicationUser user, string role);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string? password, bool requireConfirmedEmail);
        Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);
        Task<IdentityResult> CreateAccountAsync(ApplicationUser user, string password);
        Task<ApplicationUser?> FindByIdAsync(Guid userId);
        Task<ApplicationUser?> FindByNameAsync(string? email);
        Task<ApplicationUser> FindByProfileIdAsync(Guid profileId);
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user);
        Task<ResponseUserRolesAndClaims> GetUserRolesAndClaimsAsync(Guid userId);
        Task<List<string>> GetUserRolesAsync(ApplicationUser user);
        Task<string> RemoveClaimToUser(ApplicationUser user, string? claim);
        Task<string> RemoveRoleToUserAsync(ApplicationUser user, string role);


        Task<bool> IsPasswordExpiredAsync(string userId);
        Task<bool> CheckPasswordHistoryAsync(string userId, string newPassword);
        Task HandleFailedLoginAsync(string userId);
        Task ResetFailedLoginAttemptsAsync(string userId);
        Task<bool> IsAccountLockedAsync(string userId);
        Task<ApplicationUserViewModel?> GetUserDetailsAsync(Guid userId);
        Task<IdentityResult> UpdateAccountAsync(ApplicationUser user);
        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string newPassword);
        Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string oldPassword, string newPassword);
    }
}
