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

        public async Task<ResponseLoginAccount?> LoginUserUserAsync(RequestLoginAccount model)
        {
            var response = await httpService.HttpPostAsync<Response<ResponseLoginAccount>>("/api/Account/Login", model);

            if (response.Code == Status.Success) return response?.Payload;

            throw new Exception(response?.Message);
        }

        public Task<string> RegisterAccountAsync(RequestRegisterAccount model)
        {
            throw new NotImplementedException();
        }
    }
}
