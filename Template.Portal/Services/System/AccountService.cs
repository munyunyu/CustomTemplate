using Template.Library.Enums;
using Template.Library.Interface;
using Template.Library.Models;
using Template.Library.ViewsModels.System;
using Template.Portal.Interface.System;

namespace Template.Portal.Services.System
{
    public class AccountService : IAccountService
    {
        private readonly IHttpService httpService;

        public AccountService(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<ApplicationUserViewModel?> GetUserDetailsByUserIdAsnyc(string? userId, string token)
        {
            var response = await httpService.HttpGetAsync<Response<ApplicationUserViewModel>>($"/api/Account/GetUserDetails/{userId}", accessToken: token);

            if (response.Code == Status.Success) return response?.Payload;

            throw new Exception(response?.Message);
        }

        public async Task<string> GetUserIdByEmailAsync(string email, string token)
        {
            var response = await httpService.HttpGetAsync<Response<string>>($"/api/Account/GetUserIdByEmail/{email}", accessToken: token);

            if (response.Code == Status.Success) return response?.Payload ?? string.Empty;

            throw new Exception(response?.Message);
        }

        public async Task<ResponseLoginAccount?> LoginUserUserAsync(RequestLoginAccount model)
        {
            var response = await httpService.HttpPostAsync<Response<ResponseLoginAccount>>("/api/Account/Login", model);

            if (response.Code == Status.Success) return response?.Payload;

            throw new Exception(response?.Message);
        }

        public async Task<string?> RegisterAccountAsync(RequestRegisterAccount model)
        {
            var response = await httpService.HttpPostAsync<Response<ResponseRegisterAccount>>("/api/Account/Register", model);

            if (response.Code == Status.Success) return response?.Payload?.Message;

            throw new Exception(response?.Message);
        }

        public async Task<string?> UpdateAccountAsync(RequestUpdateAccount model, string token)
        {
            var response = await httpService.HttpPostAsync<Response<string>>("/api/Account/UpdateAccount", model, accessToken: token);

            if (response.Code == Status.Success) return response?.Payload;

            throw new Exception(response?.Message);
        }

        public async Task<string> UpdateUserClaimAsync(RequestUpdateUserClaimModel model, string token)
        {
            var response = await httpService.HttpPostAsync<Response<string>>("/api/Account/UpdateUserClaim", model, accessToken: token);

            if (response.Code == Status.Success) return response?.Payload ?? string.Empty;

            throw new Exception(response?.Message);
        }

        public async Task<string> UpdateUserRoleAsync(RequestUpdateUserRoleModel model, string token)
        {
            var response = await httpService.HttpPostAsync<Response<string>>("/api/Account/UpdateUserRole", model, accessToken: token);

            if (response.Code == Status.Success) return response?.Payload ?? string.Empty;

            throw new Exception(response?.Message);
        }
    }
}

