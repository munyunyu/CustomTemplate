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

        private ViewUserViewModel? _lastClickedUser;
        private ViewUserViewModel? _lastSelectedUser;


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

        // Radzen RowClick event handler
        private void OnRowClick(DataGridRowMouseEventArgs<ViewUserViewModel> args)
        {
            _lastClickedUser = args.Data;
            Console.WriteLine($"Row clicked: {args.Data?.Email}");
            StateHasChanged();
        }

        // Radzen RowSelect event handler (SelectionMode="Single")
        private void OnRowSelect(ViewUserViewModel user)
        {
            _lastSelectedUser = user;
            Console.WriteLine($"Row selected: {user?.Email}");
            StateHasChanged();
        }
        //public void OnRowSelect(ViewUserViewModel args)
        //{
        //    var record = args as ViewUserViewModel;

        //    NavigationManager.NavigateTo($"/user/details/{record.Id}", true);
        //}

        //public void OnRowClick(DataGridRowMouseEventArgs<ViewUserViewModel> args)
        //{
        //    var record = args.Data as ViewUserViewModel;

        //    NavigationManager.NavigateTo($"/user/details/{record.Id}", true);
        //}

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
