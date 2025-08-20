using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Business.Interfaces;
using Template.Database.Context;
using Template.Library.Constants;
using Template.Library.Enums;
using Template.Library.Models;
using Template.Library.ViewsModels.Profile;
using Template.Service.Extensions;

namespace Template.Service.Controllers.Profile
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IPortalService portalService;

        public ProfileController(IPortalService portalService)
        {
            this.portalService = portalService;
        }

        [HttpGet]
        [Route("GetUserProfileByUserId/{userId}")]
        public async Task<Response<ProfileViewModel>> GetUserProfileByUserId(Guid userId)
        {
            try
            {
                var profile = await portalService.Profile.GetUserProfileByUserIdAsync(userId: userId);

                return new Response<ProfileViewModel> { Code = Status.Success, Payload = profile };
            }
            catch (Exception ex)
            {
                return new Response<ProfileViewModel> { Code = Status.Failed, Message = ex.Message };
            }
        }

        [HttpGet]
        [Route("GetUserProfileByProfileId/{profileId}")]
        public async Task<Response<ProfileViewModel>> GetUserProfileByProfileId(Guid profileId)
        {
            try
            {
                var profile = await portalService.Profile.GetUserProfileByProfileIdAsync(profileId: profileId);

                return new Response<ProfileViewModel> { Code = Status.Success, Payload = profile };
            }
            catch (Exception ex)
            {
                return new Response<ProfileViewModel> { Code = Status.Failed, Message = ex.Message };
            }
        }

        [HttpGet]
        [Route("GetUserProfile")]
        public async Task<Response<ProfileViewModel>> GetUserProfile()
        {
            try
            {
                var userId = User.GetUserId();

                var users = await portalService.Profile.GetUserProfileByUserIdAsync(userId: userId);

                return new Response<ProfileViewModel> { Code = Status.Success, Payload = users };
            }
            catch (Exception ex)
            {
                return new Response<ProfileViewModel> { Code = Status.Failed, Message = ex.Message };
            }
        }

        [HttpGet]
        [Route("GetAllUserProfiles")]
        public async Task<Response<IEnumerable<ProfileViewModel>>> GetAllUserProfiles()
        {
            try
            {
                var users = await portalService.Profile.GetAllUserProfilesAsync();

                return new Response<IEnumerable<ProfileViewModel>> { Code = Status.Success, Payload = users };
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<ProfileViewModel>> { Code = Status.Failed, Message = ex.Message };
            }
        }

        [HttpPost]
        [Route("CreateProfile")]
        public async Task<Response<Guid>> CreateProfile(RequestCreateProfileModel model)
        {
            try
            {
                ApplicationUser? user = await portalService.Account.FindByIdAsync(User.GetUserId());

                ApplicationUser? user2 = await portalService.Account.FindByIdAsync(Guid.Parse(model.UserId));

                if (user == null) throw new Exception("Failed to get user claims");

                var response = await portalService.Profile.CreateProfileAsync(model, userId: user.Id);

                await portalService.Account.AddRoleToUserAsync(user, SystemRoles.Profile);

                return new Response<Guid> { Code = Status.Success, Payload = response };
            }
            catch (Exception ex)
            {
                return new Response<Guid> { Code = Status.Failed, Message = ex.Message };
            }
        }

        [HttpPost]
        [Route("UpdateProfile")]
        public async Task<Response<Guid>> UpdateEmployee(RequestUpdateProfileModel model)
        {
            try
            {
                ApplicationUser? user = await portalService.Account.FindByIdAsync(User.GetUserId());

                if (user == null) throw new Exception("Failed to get user claims");

                var response = await portalService.Profile.UpdateProfileAsync(model, userId: user.Id);

                return new Response<Guid> { Code = Status.Success, Payload = response };
            }
            catch (Exception ex)
            {
                return new Response<Guid> { Code = Status.Failed, Message = ex.Message };
            }
        }
    }
}
