using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Template.Database.Context;
using Template.Library.Models;

namespace Template.Business.Interfaces.System
{
    public interface IAccountService
    {
        Task<string> AddClaimToUser(ApplicationUser user, string? claim);
        Task<string> AddRoleToUserAsync(ApplicationUser user, string role);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string? password);
        Task<IdentityResult> CreateAccountAsync(ApplicationUser user, string? password);
        Task<ApplicationUser?> FindByIdAsync(Guid userId);
        Task<ApplicationUser> FindByNameAsync(string? email);
        Task<ApplicationUser> FindByProfileIdAsync(Guid profileId);
        Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user);
        Task<ResponseUserRolesAndClaims> GetUserRolesAndClaimsAsync(Guid userId);
        Task<List<string>> GetUserRolesAsync(ApplicationUser user);
        Task<string> RemoveClaimToUser(ApplicationUser user, string? claim);
        Task<string> RemoveRoleToUserAsync(ApplicationUser user, string role);
    }
}
