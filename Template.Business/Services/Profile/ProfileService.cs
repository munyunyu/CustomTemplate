using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Business.Interfaces.Profile;
using Template.Business.Interfaces.System;
using Template.Library.Exceptions;
using Template.Library.Models;
using Template.Library.Tables.User;
using Template.Library.ViewsModels.Profile;

namespace Template.Business.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly IDatabaseService database;
        private readonly IMapper mapper;

        public ProfileService(IDatabaseService database, IMapper mapper)
        {
            this.database = database;
            this.mapper = mapper;
        }
        public async Task<Guid> CreateProfileAsync(RequestCreateProfileModel model, string userId)
        {
            var exists = await database.ExistAsync<TblProfile>(x => x.UserId == Guid.Parse(model.UserId!));

            if (exists) throw new GeneralException($"Profile account, already exists");

            var table = new TblProfile
            {
                Id = Guid.NewGuid(),
                Description = model.Description,
                Firstname = model.Firstname,
                Email = model.Email,
                Phonenumber = model.Phonenumber,
                Surname = model.Surname,
                UserId = Guid.Parse(model.UserId!),
                LastUpdatedById = Guid.Parse(userId),
                LastUpdatedDate = DateTime.UtcNow,
                CreatedById = Guid.Parse(userId),
                CreatedDate = DateTime.UtcNow,
            };

            await database.AddAsync(table);

            return table.Id;
        }
        public async Task<IEnumerable<ProfileViewModel>> GetAllUserProfilesAsync()
        {
            string[] includes = { "Agent", "Employee", "Merchant", "Teacher", "Client", "Subscription", "Payments", "Comments", "Downloads", "Channel", "ChannelSubscribes" };

            var profile = await database.GetAllAsync<TblProfile>(includes: includes);

            return mapper.Map<IEnumerable<ProfileViewModel>>(profile);
        }

        public async Task<Guid> GetProfileIdByUserIdAsync(Guid userId)
        {
            var profile = await database.GetAsync<TblProfile>(x => x.UserId == userId);

            return profile?.Id ?? Guid.Empty;
        }

        public async Task<ProfileViewModel> GetUserProfileByProfileIdAsync(Guid profileId)
        {
            string[] includes = { "Agent", "Employee", "Merchant", "Teacher", "Client", "Subscription", "Payments", "Comments", "Downloads", "Channel", "ChannelSubscribes" };

            var profile = await database.GetAsync<TblProfile>(x => x.Id == profileId, includes);

            if (profile == null) throw new GeneralException("Profile not found, please create profile");

            return mapper.Map<ProfileViewModel>(profile);
        }

        public async Task<ProfileViewModel> GetUserProfileByUserIdAsync(Guid userId)
        {
            string[] includes = { "Agent", "Employee", "Merchant", "Teacher", "Client", "Subscription", "Payments", "Comments", "Downloads", "Channel", "Channel.ChannelPlaylists", "Channel.Videos", "Channel.ChannelSubscribes" };

            var profile = await database.GetAsync<TblProfile>(x =>x.UserId == userId, includes);

            if (profile == null) throw new GeneralException("Profile not found, please create profile");

            return mapper.Map<ProfileViewModel>(profile);
        }

        public async Task<Guid> UpdateProfileAsync(RequestUpdateProfileModel model, string userId)
        {
            var profile = await database.GetAsync<TblProfile>(x => x.Id == model.ProfileId);

            if (profile == null) throw new GeneralException("Profile not found");

            profile.Description = model.Description;
            profile.Firstname = model.Firstname;
            profile.Email = model.Email;
            profile.Phonenumber = model.Phonenumber;
            profile.Surname = model.Surname;
            profile.LastUpdatedById = Guid.Parse(userId);
            profile.LastUpdatedDate = DateTime.UtcNow;

            await database.UpdateAsync(profile);

            return profile.Id;  

        }
    }
}
