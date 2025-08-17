using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Template.Business.Interfaces;
using Template.Database.Context;
using Template.Library.Enums;
using Template.Library.Exceptions;
using Template.Library.Models;
using Template.Service.Extensions;

namespace Template.Service.Controllers.System
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IPortalService portalService;
        private readonly IConfiguration configuration;
        private readonly ILogger<AccountController> logger;

        public AccountController(IPortalService portalService, IConfiguration configuration, ILogger<AccountController> logger)
        {
            this.portalService = portalService;
            this.configuration = configuration;
            this.logger = logger;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<Response<ResponseRegisterAccount>> Register(RequestRegisterAccount model)
        {
            try
            {
                var user = new ApplicationUser { Email = model.Email, UserName = model.Email, SecurityStamp = Guid.NewGuid().ToString() };

                IdentityResult result = await portalService.Account.CreateAccountAsync(user, model.Password);

                if (!result.Succeeded) return new Response<ResponseRegisterAccount> { Code = Status.Failed, Message = string.Join("|", result.Errors.Select(x => x.Description)) };

                return new Response<ResponseRegisterAccount> { Code = Status.Success, Payload = new ResponseRegisterAccount { Email = model.Email, Message = "Account was created" } };

            }
            catch (Exception ex)
            {
                return new Response<ResponseRegisterAccount> { Code = Status.ServerError, Message = ex.Message };
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<Response<ResponseLoginAccount>> Login(RequestLoginAccount model)
        {
            try
            {
                logger.LogInformation("User email: {email} with password: {password} is logging", model.Email, model.Password);

                ApplicationUser? user = await portalService.Account.FindByNameAsync(model.Email);

                var isvalid = await portalService.Account.CheckPasswordAsync(user, model.Password);

                if (isvalid == false) return new Response<ResponseLoginAccount> { Code = Status.Failed, Message = "Email or password is not valid" };

                var user_roles = await portalService.Account.GetUserRolesAsync(user);

                var user_claims = await portalService.Account.GetUserClaimsAsync(user);


                var claims = user.GenerateUserClaims(user_roles, user_claims);

                var token = claims.GenerateJwtSecurityToken(configuration);

                return new Response<ResponseLoginAccount> 
                { 
                    Code = Status.Success, 
                    Payload = new ResponseLoginAccount 
                    { 
                        Expiration = token.ValidTo, 
                        Token = new JwtSecurityTokenHandler().WriteToken(token), 
                        UserId = user.Id,
                        Email = user.Email,
                        Phone = user.PhoneNumber,
                        Roles = user_roles
                    } };

            }
            catch (Exception ex)
            {
                return new Response<ResponseLoginAccount> { Code = Status.ServerError, Message = ex.Message };
            }
        }

        [HttpPost]
        [Route("UpdateUserClaim")]
        public async Task<Response<string>> UpdateUserClaim(RequestUpdateUserClaimModel model)
        {
            try
            {
                string message = string.Empty;

                ApplicationUser? user = await portalService.Account.FindByIdAsync(model.UserId);

                if (user == null) throw new GeneralException($"User Id: {model.UserId} was not found");

                if (model.IsAdding) message = await portalService.Account.AddClaimToUser(user: user, claim: model.ClaimName);

                else message = await portalService.Account.RemoveClaimToUser(user: user, claim: model.ClaimName);

                return new Response<string> { Code = Status.Success, Message = message, Payload = message };

            }
            catch (Exception ex)
            {
                return new Response<string> { Code = Status.ServerError, Message = ex.Message };
            }
        }

        [HttpPost]
        [Route("UpdateUserRole")]
        public async Task<Response<string>> UpdateUserRole(RequestUpdateUserRoleModel model)
        {
            try
            {
                string message = string.Empty;

                ApplicationUser? user = await portalService.Account.FindByIdAsync(model.UserId);

                if (user == null) throw new GeneralException($"User Id: {model.UserId} was not found");

                if (model.IsAdding) message = await portalService.Account.AddRoleToUserAsync(user: user, role: model.Role);

                else message = await portalService.Account.RemoveRoleToUserAsync(user: user, role: model.Role);

                return new Response<string> { Code = Status.Success, Message = message, Payload = message };

            }
            catch (Exception ex)
            {
                return new Response<string> { Code = Status.ServerError, Message = ex.Message };
            }
        }


        [HttpGet]
        [Route("GetUserRolesAndClaims/{userId}")]
        public async Task<Response<ResponseUserRolesAndClaims>> GetUserRolesAndClaims(Guid userId)
        {
            try
            {
                var users = await portalService.Account.GetUserRolesAndClaimsAsync(userId: userId);

                return new Response<ResponseUserRolesAndClaims> { Code = Status.Success, Payload = users };
            }
            catch (Exception ex)
            {
                return new Response<ResponseUserRolesAndClaims> { Code = Status.ServerError, Message = ex.Message };
            }
        }

        //[HttpGet]
        //[Route("ConfirmEmail/{email}/{token}")]
        //public async Task<Response<string>> ConfirmEmail(string email, string token)
        //{
        //    try
        //    {
        //        var user = await userManager.FindByEmailAsync(email);

        //        if (user == null) return new Response<string> { Code = Status.Failed, Message = "Failed to confirm email address" };

        //        var result = await userManager.ConfirmEmailAsync(user, token.Base64Decode());

        //        if (result.Succeeded) return new Response<string> { Code = Status.Success, Message = "Account was confirmed" };

        //        return new Response<string> { Code = Status.Failed, Message = "Failed to authenticate, please contact admin" };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Response<string> { Code = Status.ServerError, Message = ex.Message };
        //    }
        //}

        //[HttpPost]
        //[Route("ForgotPassword")]
        //public async Task<Response<ResponseForgotPasswordViewModel>> ForgotPassword(RequestForgotPasswordViewModel model)
        //{
        //    try
        //    {
        //        var user = await userManager.FindByEmailAsync(model.Email);

        //        if (user == null) return new Response<ResponseForgotPasswordViewModel> { Code = Status.Failed, Message = "Failed to confirm email address" };

        //        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        //        if (string.IsNullOrEmpty(token)) return new Response<ResponseForgotPasswordViewModel> { Code = Status.Failed, Message = "Failed to generate password reset token" };

        //        await portalService.CommunicationService.SendResetPasswordEmailToClientAsync(email: model.Email, userId: user.Id, token: token.Base64Encode());

        //        return new Response<ResponseForgotPasswordViewModel> { Code = Status.Success, Payload = new ResponseForgotPasswordViewModel { Message = "Check your email to reset the password" } };

        //    }
        //    catch (Exception ex)
        //    {
        //        return new Response<ResponseForgotPasswordViewModel> { Code = Status.ServerError, Message = ex.Message };
        //    }
        //}

        //[HttpPost]
        //[Route("ChangePassword")]
        //public async Task<Response<ResponseChangePasswordViewModel>> ChangePassword(RequestChangePasswordViewModel model)
        //{
        //    try
        //    {
        //        var user = await userManager.FindByEmailAsync(model.Email);

        //        if (user == null) return new Response<ResponseChangePasswordViewModel> { Code = Status.Failed, Message = "Failed to confirm email address" };

        //        IdentityResult result = await userManager.ResetPasswordAsync(user: user, token: model.Token.Base64Decode(), newPassword: model.NewPassword);

        //        if (!result.Succeeded) return new Response<ResponseChangePasswordViewModel> { Code = Status.Failed, Message = string.Join("|", result.Errors.Select(x => x.Description)) };

        //        return new Response<ResponseChangePasswordViewModel> { Code = Status.Success, Payload = new ResponseChangePasswordViewModel { Message = "Password was changed" } };

        //    }
        //    catch (Exception ex)
        //    {
        //        return new Response<ResponseChangePasswordViewModel> { Code = Status.ServerError, Message = ex.Message };
        //    }

        //}
    }
}
