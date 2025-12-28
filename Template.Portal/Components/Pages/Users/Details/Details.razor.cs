using Microsoft.AspNetCore.Components;
using Template.Library.Extensions;
using Template.Library.ViewsModels.System;

namespace Template.Portal.Components.Pages.Users.Details
{
    public partial class Details
    {
        [Parameter]
        public string? UserId { get; set; }

        public ApplicationUserViewModel? User { get; set; } = new ApplicationUserViewModel();


        protected override async Task OnInitializedAsync()
        {
            try
            {
                HelperService.SetIsLoadingState(true);

                var token = await AuthService.GetCurrentUserTokenAsync();

                User = await PortalService.Account.GetUserDetailsByUserIdAsnyc(userId: UserId, token: token);

                if (User == null) throw new Exception($"User with Id: {UserId} found");

                HelperService.SetIsLoadingState(false);
            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex);
            }
        }



        protected async Task HandleValidUpdatePassword()
        {
            try
            {
                HelperService.SetIsLoadingState(true);

                //await PortalService.Account.UpdatePasswordAsync(Guid.Parse(UserId), UserModel.Password);

                HelperService.SetSuccessMessage("Password has been changed");

                HelperService.SetIsLoadingState(false);
            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex.GetAllMessages());
            }
        }
    }
}
