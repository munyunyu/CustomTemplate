using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MVA.Business.Interface;
using MVA.Library.Models.Client;
using MVA.Portal.Services;
using Radzen;
using MVA.Library.Extensions;
using MVA.Portal.Extensions;
using MVA.Portal.Pages.Helper;
using MVA.Library.Tables;
using MVA.Library.Views;


namespace MVA.Portal.Pages.Users.Index
{
    public partial class Index : BasePage
    {
        public IEnumerable<ViewApplicationUser> Users { get; set; } = new List<ViewApplicationUser>();


        protected override async Task OnInitializedAsync()
        {
            try
            {
                HelperService.SetIsLoadingState(true);

                Users = await PortalService.Account.GetAllUsersAsnyc();

                HelperService.SetIsLoadingState(false);
            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex);
            }
        }

        public void RowClicked(object args)
        {
            var record = (ViewApplicationUser)args;

            NavigationManager.NavigateTo($"/user/details/{record.Id}", true);
        }

        public void RowRender(RowRenderEventArgs<ViewApplicationUser> args)
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
