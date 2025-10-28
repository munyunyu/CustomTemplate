using Microsoft.AspNetCore.Components;
using Radzen;
using Template.Library.Tables.Views;
using Template.Library.ViewsModels.System;
using Template.Portal.Pages.Helper;


namespace Template.Portal.Pages.Users.Index
{
    public partial class Index : BasePage
    {
        public IEnumerable<SystemUserViewModel> Users { get; set; } = new List<SystemUserViewModel>();


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
            var record = (SystemUserViewModel)args;

            NavigationManager.NavigateTo($"/user/details/{record.Id}", true);
        }

        public void RowRender(RowRenderEventArgs<SystemUserViewModel> args)
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
