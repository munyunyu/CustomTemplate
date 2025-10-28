using Template.Library.Enums;
using Template.Library.Interface;
using Template.Library.Models;
using Template.Library.ViewsModels.System;
using Template.Portal.Interface.System;

namespace Template.Portal.Services.System
{
    public class AdminService : IAdminService
    {
        private readonly IHttpService httpService;

        public AdminService(IHttpService httpService)
        {
            this.httpService = httpService;
        }
        public async Task<IEnumerable<SystemUserViewModel>> GetAllUsersAsnyc()
        {
            var response = await httpService.HttpGetAsync<Response<IEnumerable<SystemUserViewModel>>>("/api/Admin/GetSystemUsers");

            if (response.Code == Status.Success) return response?.Payload ?? Enumerable.Empty<SystemUserViewModel>();

            throw new Exception(response?.Message);
        }
    }
}
