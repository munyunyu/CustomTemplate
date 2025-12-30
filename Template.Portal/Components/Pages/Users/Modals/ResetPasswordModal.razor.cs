using Microsoft.AspNetCore.Components;
using Template.Library.Models;
using Template.Portal.Components.Shared.Helpers;
using Template.Portal.Extensions;

namespace Template.Portal.Components.Pages.Users.Modals
{
    public partial class ResetPasswordModal : BasePage
    {
        [Parameter]
        public string UserId { get; set; } = string.Empty;
        public RequestResetPassword Model { get; set; } = new RequestResetPassword();

        override protected void OnInitialized()
        {
            base.OnInitialized();

            Model.UserId = Guid.Parse(UserId);
        }

        public async Task HandleValidUserResetPasswordSubmit()
        {
            try
            {
                HelperService.SetIsLoadingState(true);

                var token = await AuthService.GetCurrentUserTokenAsync();

                var message = await PortalService.Account.ResetPasswordAsync(Model, token);

                HelperService.SetSuccessMessage(message ?? "User password reset was successfully.");

                NavigationManager.ReloadPage();

            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex);
            }
        }
    }
}
