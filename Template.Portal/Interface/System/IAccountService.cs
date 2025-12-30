using Template.Library.Models;
using Template.Library.ViewsModels.System;

namespace Template.Portal.Interface.System
{
    public interface IAccountService
    {
        Task<string?> ChangePasswordAsync(RequestChangePassword model, string token);
        Task<ApplicationUserViewModel?> GetUserDetailsByUserIdAsnyc(string? userId, string token);
        Task<string> GetUserIdByEmailAsync(string email, string token);
        Task<ResponseLoginAccount?> LoginUserUserAsync(RequestLoginAccount model);
        Task<string?> RegisterAccountAsync(RequestRegisterAccount model);
        Task<string?> ResetPasswordAsync(RequestResetPassword model, string token);
        Task<string?> UpdateAccountAsync(RequestUpdateAccount model, string token);
        Task<string> UpdateUserClaimAsync(RequestUpdateUserClaimModel model, string token);
        Task<string> UpdateUserRoleAsync(RequestUpdateUserRoleModel model, string token);
    }
}
