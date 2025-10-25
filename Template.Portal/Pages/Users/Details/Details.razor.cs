using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Components;
using MVA.Library.Extensions;
using MVA.Library.Models.Account;
using MVA.Library.Views;
using MVA.Portal.Extensions;
using MVA.Portal.Pages.Helper;

namespace MVA.Portal.Pages.Users.Details
{
    public partial class Details : BasePage
    {
        [Parameter]
        public string? UserId { get; set; }

        public ViewApplicationUser? User { get; set; } = new ViewApplicationUser();
        public UserViewModel2 UserModel { get; set; } = new UserViewModel2();
        public List<UserRoleViewModel> Roles { get; set; } = new List<UserRoleViewModel>();
        public string SelectedAccordation { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                HelperService.SetIsLoadingState(true);

                User = await PortalService.Account.GetUserDetailsByUserIdAsnyc(userId: UserId);

                if (User == null) throw new Exception($"User with Id: {UserId} found");

                UserModel = (await PortalService.Account.GetUserRolesByUserIdAsync(userId: UserId)).ToModelUserRoles(User);

                Roles = await PortalService.Account.GetSystemRolesAsync();

                foreach (var role in Roles) role.Checked = UserModel.UserRoles.Exists(r => r.Id == role.Id);

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

                await PortalService.Account.UpdatePasswordAsync(Guid.Parse(UserId), UserModel.Password);

                HelperService.SetSuccessMessage("Password has been changed");

                HelperService.SetIsLoadingState(false);
            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex.GetAllMessages());
            }
        }

        protected void AccordationHeaderSelected(string args)
        {
            if (SelectedAccordation == args)
            {
                SelectedAccordation = string.Empty;
                return;
            }
            SelectedAccordation = args;
        }

        private async Task RoleChange(UserRoleViewModel role, ChangeEventArgs e)
        {
            try
            {
                HelperService.SetIsLoadingState(true);

                role.Checked = (bool)e.Value;

                if (role.Checked) await PortalService.Account.AssignRoleAsync(UserModel.Id, role.Id);

                else await PortalService.Account.UnassignRoleAsync(UserModel.Id, role.Id);

                HelperService.SetSuccessMessage("Successfully updated user roles");

                NavigationManager.ReloadPage();
            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex.GetAllMessages());
            }
        }
    }
}
