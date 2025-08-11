using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Library.Models;
using Template.Library.ViewsModels.Profile;

namespace Template.Business.Interfaces.Profile
{
    public interface IProfileService
    {
        Task<Guid> CreateProfileAsync(RequestCreateProfileModel model, string userId);
        Task<IEnumerable<ProfileViewModel>> GetAllUserProfilesAsync();
        Task<Guid> GetProfileIdByUserIdAsync(Guid userId);
        Task<ProfileViewModel> GetUserProfileByProfileIdAsync(Guid profileId);
        Task<ProfileViewModel> GetUserProfileByUserIdAsync(Guid userId);
        Task<Guid> UpdateProfileAsync(RequestUpdateProfileModel model, string userId);
    }
}
