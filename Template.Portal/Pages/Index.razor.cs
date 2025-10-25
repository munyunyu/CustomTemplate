using Microsoft.AspNetCore.Components;
using Radzen;
using Template.Portal.Interface;
using Template.Portal.Services.System;

namespace Template.Portal.Pages
{
    public partial class Index
    {        
        [Inject] public required NavigationManager NavigationManager { get; set; }
        [Inject] public required IPortalService PortalService { get; set; }
        [Inject] public required HelperService HelperService { get; set; }
        [Inject] public AuthService? AuthService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                HelperService.SetIsLoadingState(true);

               
                HelperService.SetIsLoadingState(false);
            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex);
            }
        }


        public void RowClicked(object args)
        {
            //var record = (LoanApplicationViewModel)args;

            //NavigationManager.NavigateTo($"/loan/details/{record.Id}", true);
        }

        //public void RowRender(RowRenderEventArgs<LoanApplicationViewModel> args)
        //{
        //    try
        //    {
        //        if (args.Data.LoanRepaymentStatus == LoanRepaymentStatus.Completed) args.Attributes.Add("Style", $"background-color:{Library.Constants.Colors.Green};");
        //    }
        //    catch (Exception)
        //    {
        //        //do nothing
        //    }

        //}

       
      
       
    }
}
