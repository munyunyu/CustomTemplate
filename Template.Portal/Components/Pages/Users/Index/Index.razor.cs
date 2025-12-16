using Microsoft.AspNetCore.Components;
using Radzen;
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

        public void RowClicked(object args)
        {
            var record = (ViewUserViewModel)args;

            NavigationManager.NavigateTo($"/user/details/{record.Id}", true);
        }

        public void RowRender(RowRenderEventArgs<ViewUserViewModel> args)
        {
            try
            {
                //var color = args.Data.GetRowRenderColor();

                //args.Attributes.Add("Style", $"background-color:{color};");

            }
            catch (Exception)
            {
                //mark it black
            }

        }

        public void OnClickStatusColor(string color)
        {

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
