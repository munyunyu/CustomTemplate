using Template.Library.Models;

namespace Template.Portal.Interface.System
{
    public interface IAccountService
    {
        Task<ResponseLoginAccount> LoginUserUserAsync(RequestLoginAccount model);
        Task<string> RegisterAccountAsync(RequestRegisterAccount model);
    }
}
