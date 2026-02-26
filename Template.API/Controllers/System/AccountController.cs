using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Template.Business.Interfaces;
using Template.Database.Context;
using Template.Library.Constants;
using Template.Library.Enums;
using Template.Library.Exceptions;
using Template.Library.Extensions;
using Template.Library.Models;
using Template.Library.Tables.User;
using Template.Library.ViewsModels.System;
using Template.Service.Extensions;

namespace Template.Service.Controllers.System
{
    [Authorize]
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
        [Route("UpdateAccount")]
        public async Task<Response<string>> UpdateAccount(RequestUpdateAccount model)
        {
            try
            {
                var user = await portalService.Account.FindByIdAsync(model.UserId);

                if (user == null) throw new GeneralException($"Failed to find userId: {model.UserId}");

                if(!string.IsNullOrEmpty(model.FirstName)) user.FirstName = model.FirstName;
                if (!string.IsNullOrEmpty(model.LastName)) user.LastName = model.LastName;
                if (!string.IsNullOrEmpty(model.Description)) user.Description = model.Description;
                if (!string.IsNullOrEmpty(model.PhoneNumber)) user.PhoneNumber = model.PhoneNumber;

                user.LastUpdatedDate = DateTime.UtcNow;
                user.LastUpdatedById = User.GetUserId();                
                user.PhoneNumberConfirmed = false;             

                IdentityResult result = await portalService.Account.UpdateAccountAsync(user);

                if (!result.Succeeded) return new Response<string> { Code = Status.Failed, Message = string.Join("|", result.Errors.Select(x => x.Description)) };
                               
                return new Response<string> { Code = Status.Success, Payload = "Account was updated" };

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "UserId: {UserId} failed to update account", model.UserId);

                return new Response<string> { Code = Status.Failed, Message = ex.Message };
            }
        }

        //[AllowAnonymous]
        [Authorize(Roles = SystemRoles.Admin)]
        [HttpPost]
        [Route("Register")]
        public async Task<Response<ResponseRegisterAccount>> Register(RequestRegisterAccount model)
        {
            try
            {
                var user = new ApplicationUser 
                { 
                    Email = model.Email, 
                    UserName = model.Email, 
                    SecurityStamp = Guid.NewGuid().ToString(),
                    CreatedDate = DateTime.UtcNow,
                    LastUpdatedDate = DateTime.UtcNow,
                    FirstName = model.FirstName,
                    LastName = model.LastName, 
                    PhoneNumber = model.PhoneNumber
                };

                IdentityResult result = await portalService.Account.CreateAccountAsync(user, model.Password);

                if (!result.Succeeded) return new Response<ResponseRegisterAccount> { Code = Status.Failed, Message = string.Join("|", result.Errors.Select(x => x.Description)) };

                var token = await portalService.Account.GenerateEmailConfirmationTokenAsync(user);

                await portalService.Communication.SendConfirmEmailAsync(to: model.Email, template_name: EmailTemplat.ConfirmEmail, token: token, config_name: EmailConfig.Default);

                return new Response<ResponseRegisterAccount> { Code = Status.Success, Payload = new ResponseRegisterAccount { Email = model.Email, Message = "Account was created, please confirm your email" } };

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "User email: {email} failed to register", model.Email);

                return new Response<ResponseRegisterAccount> { Code = Status.Failed, Message = ex.Message };
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<Response<ResponseLoginAccount>> Login(RequestLoginAccount model)
        {
            try
            {
                //portalService.Rabbit.Publish(RabbitQueue.GeneralEmailNotification, model.Email, true);

                ApplicationUser? user = await portalService.Account.FindByNameAsync(model.Email);

                if (user == null) return new Response<ResponseLoginAccount> { Code = Status.Failed, Message = "Email or password is not valid" };

                var isvalid = await portalService.Account.CheckPasswordAsync(user, model.Password, requireConfirmedEmail: true);

                if (isvalid == false)
                {
                    logger.LogWarning("User email: {email} failed to login", model.Email);

                    return new Response<ResponseLoginAccount> { Code = Status.Failed, Message = "Email or password is not valid" };
                }

                var user_roles = await portalService.Account.GetUserRolesAsync(user);

                var user_claims = await portalService.Account.GetUserClaimsAsync(user);

                var claims = user.GenerateUserClaims(user_roles, user_claims);

                var token = claims.GenerateJwtSecurityToken(configuration);

                logger.LogInformation("User email: {email} successully logged-in", model.Email);

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
                        Roles = user_roles,
                        Claims = user_claims?.Select(x => x.Value).ToList()
                    } };

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "User email: {email} failed to login", model.Email);

                return new Response<ResponseLoginAccount> { Code = Status.Failed, Message = ex.Message };
            }
        }

        [HttpPost]
        [Route("UpdateUserClaim")]
        [Authorize(Policy = SystemPolicy.AdminPolicyUpdate)]
        public async Task<Response<string>> UpdateUserClaim(RequestUpdateUserClaimModel model)
        {
            try
            {
                string message = string.Empty;

                ApplicationUser? user = await portalService.Account.FindByIdAsync(model.UserId);

                if (user == null) throw new GeneralException($"User Id: {model.UserId} was not found");

                if(User.GetUserId() == Guid.Parse(user.Id)) throw new GeneralException("You cannot add or remove your own claim");

                if (model.IsAdding) message = await portalService.Account.AddClaimToUser(user: user, claim: model.ClaimName);

                else message = await portalService.Account.RemoveClaimToUser(user: user, claim: model.ClaimName);

                return new Response<string> { Code = Status.Success, Message = message, Payload = message };

            }
            catch (Exception ex)
            {
                return new Response<string> { Code = Status.Failed, Message = ex.Message };
            }
        }

        [HttpPost]
        [Route("UpdateUserRole")]
        [Authorize(Policy = SystemPolicy.AdminPolicyUpdate)]
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
                return new Response<string> { Code = Status.Failed, Message = ex.Message };
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
                return new Response<ResponseUserRolesAndClaims> { Code = Status.Failed, Message = ex.Message };
            }
        }

        [HttpGet]
        [Route("GetUserDetails/{userId}")]
        public async Task<Response<ApplicationUserViewModel>> GetUserDetails(Guid userId)
        {
            try
            {
                var users = await portalService.Account.GetUserDetailsAsync(userId: userId);

                return new Response<ApplicationUserViewModel> { Code = Status.Success, Payload = users };
            }
            catch (Exception ex)
            {
                return new Response<ApplicationUserViewModel> { Code = Status.Failed, Message = ex.Message };
            }
        }

        [HttpGet]
        [Route("ConfirmEmail/{email}/{token}")]
        public async Task<Response<string>> ConfirmEmail(string email, string token)
        {
            try
            {
                var user = await portalService.Account.FindByNameAsync(email);

                if (user == null) return new Response<string> { Code = Status.Failed, Message = "Failed to confirm email address" };

                IdentityResult result = await portalService.Account.ConfirmEmailAsync(user, token.Base64Decode());

                if (result.Succeeded) return new Response<string> { Code = Status.Success, Message = "Account was confirmed" };

                logger.LogWarning("User email: {email} failed to confirm email address", email);

                return new Response<string> { Code = Status.Failed, Message = "Failed to authenticate, please contact admin" };
            }
            catch (Exception ex)
            {
                return new Response<string> { Code = Status.Failed, Message = ex.Message };
            }
        }

        [HttpGet]
        [Route("GetUserIdByEmail/{email}")]
        public async Task<Response<string>> GetUserIdByEmail(string email)
        {
            try
            {
                var user = await portalService.Account.FindByNameAsync(email);

                if (user == null) return new Response<string> { Code = Status.Failed, Message = $"User with email: {email} address was not found" };

                return new Response<string> { Code = Status.Success, Payload = user.Id };
            }
            catch (Exception ex)
            {
                return new Response<string> { Code = Status.Failed, Message = ex.Message };
            }
        }

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

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<Response<string>> ChangePassword(RequestChangePassword model)
        {
            try
            {
                var user = await portalService.Account.FindByIdAsync(model.UserId);

                if (user == null) throw new GeneralException($"UserId: {model.UserId} was not found");

                IdentityResult result = await portalService.Account.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (!result.Succeeded) return new Response<string> { Code = Status.Failed, Message = string.Join("|", result.Errors.Select(x => x.Description)) };

                user.LastUpdatedDate = DateTime.UtcNow;
                user.LastUpdatedById = User.GetUserId();
                user.LastPasswordChangeDate = DateTime.UtcNow;

                if (user.PasswordNeverExpires) user.PasswordExpiryDate = DateTime.UtcNow.AddDays(60);

                await portalService.Account.UpdateAccountAsync(user);

                return new Response<string> { Code = Status.Success, Payload = "Password was changed" };

            }
            catch (Exception ex)
            {
                return new Response<string> { Code = Status.Failed, Message = ex.Message };
            }

        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<Response<string>> ResetPassword(RequestResetPassword model)
        {
            try
            {
                var user = await portalService.Account.FindByIdAsync(model.UserId);

                if (user == null) throw new GeneralException($"UserId: {model.UserId} was not found");

                IdentityResult result = await portalService.Account.ResetPasswordAsync(user, model.NewPassword);

                if (!result.Succeeded) return new Response<string> { Code = Status.Failed, Message = string.Join("|", result.Errors.Select(x => x.Description)) };

                user.LastUpdatedDate = DateTime.UtcNow;
                user.LastUpdatedById = User.GetUserId();
                user.LastPasswordChangeDate = DateTime.UtcNow;                
                user.PasswordNeverExpires = model.PasswordNeverExpires;

                if (model.PasswordNeverExpires) user.PasswordExpiryDate = null;
                else user.PasswordExpiryDate = model.PasswordExpiryDate;

                await portalService.Account.UpdateAccountAsync(user);

                //send email to the client with the password

                return new Response<string> { Code = Status.Success, Payload = "Password was changed" };

            }
            catch (Exception ex)
            {
                return new Response<string> { Code = Status.Failed, Message = ex.Message };
            }

        }
    }
}
