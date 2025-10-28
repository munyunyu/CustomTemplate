using Template.Library.Models;

namespace Template.Portal.Interface.System
{
    public interface IAccountService
    {
        Task<string> GetUserIdByEmailAsync(string email, string token);
        Task<ResponseLoginAccount?> LoginUserUserAsync(RequestLoginAccount model);
        Task<string?> RegisterAccountAsync(RequestRegisterAccount model);
    }
}
