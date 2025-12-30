using Microsoft.AspNetCore.Components;
using Template.Library.Models;
using Template.Portal.Components.Shared.Helpers;
using Template.Portal.Extensions;

namespace Template.Portal.Components.Pages.Users.Modals
{
    public partial class UpdateUserModal : BasePage
    {
        [Parameter]
        public string UserId { get; set; } = string.Empty;
        public RequestUpdateAccount Model { get; set; } = new RequestUpdateAccount();

        override protected void OnInitialized()
        {
            base.OnInitialized();

            Model.UserId = Guid.Parse(UserId);
        }

        public async Task HandleValidUserUpdateSubmit()
        {
            try
            {
                HelperService.SetIsLoadingState(true);

                var token = await AuthService.GetCurrentUserTokenAsync();

                var message = await PortalService.Account.UpdateAccountAsync(Model, token);

                HelperService.SetSuccessMessage(message ?? "User updated successfully.");

                NavigationManager.ReloadPage();
            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex);
            }
        }
    }
}
