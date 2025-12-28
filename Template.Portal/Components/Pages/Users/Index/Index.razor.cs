using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Template.Library.ViewsModels.System;
using Template.Portal.Components.Shared.Helpers;
using Template.Portal.Services;
using Template.Portal.Services.System;

namespace Template.Portal.Components.Pages.Users.Index
{
    public partial class Index : BasePage
    {
        public IEnumerable<ViewUserViewModel> Users { get; set; } = new List<ViewUserViewModel>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                HelperService.SetIsLoadingState(true);

                Users = await PortalService.Admin.GetAllUsersAsnyc();

                HelperService.SetIsLoadingState(false);
            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex);
            }
        }

        private void OnRowClick(DataGridRowMouseEventArgs<ViewUserViewModel> args)
        {
            NavigationManager.NavigateTo($"/user/details/{args.Data.Id}", true);
        }

        public void RowRender(Radzen.RowRenderEventArgs<ViewUserViewModel> args)
        {
            try
            {
                if (args.Data?.EmailConfirmed == false)
                {
                    args.Attributes["style"] = "background-color: #ffe7e7;";
                }
            }
            catch (Exception)
            {
                //mark it black
            }

        }

        protected async Task GenerateExport()
        {
            try
            {
                HelperService.SetIsLoadingState(true);


                HelperService.SetIsLoadingState(false);
            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex.Message);
            }
        }
    }
}
