using AutoMapper;
using Microsoft.AspNetCore.Identity;
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

namespace Template.Business.Services.System
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDatabaseService database;
        public AccountService(UserManager<ApplicationUser> userManager, IDatabaseService database)
        {
            this.userManager = userManager;
            this.database = database;
        }

        public async Task<string> AddClaimToUser(ApplicationUser user, string? claimType)
        {
            var exits = new SystemClaims().GetConstantValues().Any(x =>  x == claimType);

            if (exits == false) throw new GeneralException($"Claim: {claimType} was not found");
            
            var claims = await userManager.GetClaimsAsync(user); // Check if the user has the specified claim

            if(claims.Any(c => c.Type == claimType)) throw new GeneralException($"Claim: {claimType} already exists");

            var claim = new Claim(SystemClaims.AdminCreate, SystemClaims.AdminCreate);

            var result = await userManager.AddClaimAsync(user, claim);

            if (result.Succeeded) return "Success";

            throw new GeneralException($"Failed to add claim: {claimType}");
        }

        public async Task<string> RemoveClaimToUser(ApplicationUser user, string? claimType)
        {
            var exits = new SystemClaims().GetConstantValues().Any(x => x == claimType);

            if (exits == false) throw new GeneralException($"Claim: {claimType} was not found");

            var claims = await userManager.GetClaimsAsync(user);

            if (claims.Any(c => c.Type == claimType) == false) throw new GeneralException($"Claim: {claimType} was not found for this user");

            var claim = new Claim(SystemClaims.AdminCreate, SystemClaims.AdminCreate);

            var result = await userManager.RemoveClaimsAsync(user, claims.ToArray());

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

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string? password)
        {
            if (string.IsNullOrEmpty(password)) throw new Exception("password is required");

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
                CreatedDate = DateTime.Now,
                LastUpdatedById = Guid.Parse(user.Id),
                LastUpdatedDate = DateTime.Now,
            });

            return isvalid;
        }

        public Task<IdentityResult> CreateAccountAsync(ApplicationUser user, string? password)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser?> FindByIdAsync(Guid userId)
        {
            ApplicationUser? user = await userManager.FindByIdAsync(userId.ToString());

            //if (user == null) throw new Exception($"User with userId: {userId} was not found");

            return user;
        }

        public async Task<ApplicationUser> FindByNameAsync(string? email)
        {
            if (string.IsNullOrEmpty(email)) throw new Exception("Email is  required");

            ApplicationUser? user = await userManager.FindByNameAsync(email);

            if (user == null) throw new Exception("Email or password is not valid");

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
    }
}
