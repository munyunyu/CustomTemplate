using Template.Library.Models;
using Template.Library.ViewsModels.System;

namespace Template.Portal.Interface.System
{
    public interface IAccountService
    {
        Task<ApplicationUserViewModel?> GetUserDetailsByUserIdAsnyc(string? userId, string token);
        Task<string> GetUserIdByEmailAsync(string email, string token);
        Task<ResponseLoginAccount?> LoginUserUserAsync(RequestLoginAccount model);
        Task<string?> RegisterAccountAsync(RequestRegisterAccount model);
    }
}
