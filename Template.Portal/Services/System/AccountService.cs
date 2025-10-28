using Template.Library.Enums;
using Template.Library.Interface;
using Template.Library.Models;
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
    }
}
