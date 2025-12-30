using Microsoft.AspNetCore.Components;
using Template.Library.Models;
using Template.Portal.Components.Shared.Helpers;
using Template.Portal.Extensions;

namespace Template.Portal.Components.Pages.Users.Modals
{
    public partial class ChangePasswordModal : BasePage
    {
        [Parameter]
        public string UserId { get; set; } = string.Empty;
        public RequestChangePassword Model { get; set; } = new RequestChangePassword();

        override protected void OnInitialized()
        {
            base.OnInitialized();

            Model.UserId = Guid.Parse(UserId);
        }

        public async Task HandleValidUserChangePasswordSubmit()
        {
            try
            {
                HelperService.SetIsLoadingState(true);

                var token = await AuthService.GetCurrentUserTokenAsync();

                var message = await PortalService.Account.ChangePasswordAsync(Model, token);

                HelperService.SetSuccessMessage(message ?? "User password was changed successfully.");

                NavigationManager.ReloadPage();
            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex);
            }
        }
    }
}
