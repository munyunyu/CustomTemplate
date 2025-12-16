using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using Template.Library.Models;
using Template.Portal.Components.Shared.Helpers;
using Template.Portal.Extensions;

namespace Template.Portal.Components.Pages.Users.Modals
{
    public partial class UpdateUserClaim : BasePage
    {
        [Parameter]
        public string UserId { get; set; } = string.Empty;
        public RequestUpdateUserClaimModel Model { get; set; } = new RequestUpdateUserClaimModel();

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

                string message = await PortalService.Account.UpdateUserClaimAsync(model: Model, token: token);

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
