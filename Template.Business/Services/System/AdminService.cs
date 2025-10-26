using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Business.Interfaces.System;
using Template.Library.Models;
using Template.Library.Tables.Views;
using Template.Library.Views.System;
using Template.Library.ViewsModels.System;

namespace Template.Business.Services.System
{
    public class AdminService : IAdminService
    {
        private readonly IDatabaseService databaseService;
        private readonly IMapper mapper;

        public AdminService(IDatabaseService databaseService, IMapper mapper)
        {
            this.databaseService = databaseService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SystemUserViewModel>?> GetSystemUsersAsync()
        {
            var records = await databaseService.GetAllAsync<ViewApplicationUser>();

            return mapper.Map<IEnumerable<SystemUserViewModel>>(records);
        }

        public async Task<IEnumerable<SystemUserRolesViewModel>?> GetUsersRolesAsync()
        {
            var records = await databaseService.GetAllAsync<ViewSystemUserRoles>();

            return mapper.Map<IEnumerable<SystemUserRolesViewModel>>(records);
        }
    }
}
