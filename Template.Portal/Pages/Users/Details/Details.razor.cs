using Microsoft.AspNetCore.Components;
using Template.Library.Extensions;
using Template.Library.ViewsModels.System;
using Template.Portal.Extensions;
using Template.Portal.Pages.Helper;

namespace Template.Portal.Pages.Users.Details
{
    public partial class Details : BasePage
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

      

        //private async Task RoleChange(UserRoleViewModel role, ChangeEventArgs e)
        //{
        //    try
        //    {
        //        HelperService.SetIsLoadingState(true);

        //        role.Checked = (bool)e.Value;

        //        if (role.Checked) await PortalService.Account.AssignRoleAsync(UserModel.Id, role.Id);

        //        else await PortalService.Account.UnassignRoleAsync(UserModel.Id, role.Id);

        //        HelperService.SetSuccessMessage("Successfully updated user roles");

        //        NavigationManager.ReloadPage();
        //    }
        //    catch (Exception ex)
        //    {
        //        HelperService.SetErrorMessage(ex.GetAllMessages());
        //    }
        //}
    }
}
