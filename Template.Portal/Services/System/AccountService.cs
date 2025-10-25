using Template.Library.Models;
using Template.Portal.Interface.System;

namespace Template.Portal.Services.System
{
    public class AccountService : IAccountService
    {
        public Task<ResponseLoginAccount> LoginUserUserAsync(RequestLoginAccount model)
        {
            throw new NotImplementedException();
        }

        public Task<string> RegisterAccountAsync(RequestRegisterAccount model)
        {
            throw new NotImplementedException();
        }
    }
}
