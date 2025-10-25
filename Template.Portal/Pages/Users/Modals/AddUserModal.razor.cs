using MVA.Library.Models.Account;
using MVA.Portal.Pages.Helper;

namespace MVA.Portal.Pages.Users.Modals
{
    public partial class AddUserModal : BasePage
    {
        public RequestRegisterAccount Model { get; set; } = new RequestRegisterAccount();

        public async Task HandleValidUserRegistrationSubmit()
        {
            try
            {
                HelperService.SetIsLoadingState(true);

                var userId = await PortalService.Account.RegisterAccountAsync(Model);

                HelperService.SetSuccessMessage(string.Empty);

                NavigationManager.NavigateTo($"/user/details/{userId}", true);
            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex);
            }
        }
    }
}
