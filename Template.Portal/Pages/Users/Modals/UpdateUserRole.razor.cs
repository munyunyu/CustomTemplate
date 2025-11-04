using Microsoft.AspNetCore.Components;
using Template.Library.Models;
using Template.Portal.Extensions;
using Template.Portal.Pages.Helper;

namespace Template.Portal.Pages.Users.Modals
{
    public partial class UpdateUserRole : BasePage
    {
        [Parameter]
        public string UserId { get; set; } = string.Empty;

        public RequestUpdateUserRoleModel Model { get; set; } = new RequestUpdateUserRoleModel();

        override protected void OnInitialized()
        {
            base.OnInitialized();

            Model.UserId = Guid.Parse(UserId);
        }

        public async Task HandleValidUserRegistrationSubmit()
        {
            try
            {
                HelperService.SetIsLoadingState(true);

                var token = await AuthService.GetCurrentUserTokenAsync();

                string message = await PortalService.Account.UpdateUserRoleAsync(model: Model, token: token);

                HelperService.SetSuccessMessage(message);

                NavigationManager.ReloadPage();
            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex);
            }
        }
    }
}
