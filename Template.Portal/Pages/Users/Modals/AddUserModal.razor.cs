using System.ComponentModel.DataAnnotations;
using Template.Library.Models;
using Template.Portal.Pages.Helper;

namespace Template.Portal.Pages.Users.Modals
{
    public partial class AddUserModal : BasePage
    {
        public RequestRegisterAccount Model { get; set; } = new RequestRegisterAccount();

        public async Task HandleValidUserRegistrationSubmit()
        {
            try
            {
                HelperService.SetIsLoadingState(true);

                var message = await PortalService.Account.RegisterAccountAsync(Model);

                HelperService.SetSuccessMessage(message ?? "User registered successfully.");

                var token = await AuthService.GetCurrentUserTokenAsync();

                string userId = await PortalService.Account.GetUserIdByEmailAsync(email: Model.Email, token: token);

                NavigationManager.NavigateTo($"/user/details/{userId}", true);
            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex);
            }
        }
    }
}
