using Microsoft.AspNetCore.Components;
using Template.Library.ViewsModels.System;
using Template.Portal.Components.Shared.Helpers;
using Template.Portal.Services;
using Template.Portal.Services.System;
using Template.Portal.Components.Shared.DataTable;

namespace Template.Portal.Components.Pages.Users.Index
{
    public partial class Index : BasePage
    {
        public IEnumerable<ViewUserViewModel> Users { get; set; } = new List<ViewUserViewModel>();

        private ViewUserViewModel? _lastClickedUser;


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

        // RowClick event handler
        private void OnRowClick(ViewUserViewModel user)
        {
            _lastClickedUser = user;
            Console.WriteLine($"Row clicked: {user?.Email}");
            StateHasChanged();
        }

        // RowSelect event handler (for single selection mode)
        private void OnRowSelect(ViewUserViewModel user)
        {
            Console.WriteLine($"Row selected: {user?.Email}");

            //selectedUser = user;

            //// If you need to programmatically select/deselect
            //if (dataGridRef != null && user != null)
            //{
            //    // This ensures the grid knows about the selection
            //    dataGridRef.SelectRow(user);
            //}

            //StateHasChanged();
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
