using Microsoft.AspNetCore.Components;
using MVA.Business.Interface;
using MVA.Library.Enums;
using MVA.Library.Extensions;
using MVA.Library.Models.Batch;
using MVA.Library.Models.Loan;
using MVA.Library.Models.Statistics;
using MVA.Library.Models.Worker;
using MVA.Portal.Extensions;
using MVA.Portal.Services;
using Radzen;

namespace MVA.Portal.Pages
{
    public partial class Index
    {
        public IEnumerable<ServiceRunningViewModel> Workers { get; set; } = new List<ServiceRunningViewModel>();
        public DashboardStatistics01Model DashboardStatistics01 { get; set; } = new DashboardStatistics01Model();
        public IEnumerable<LoanApplicationViewModel> LoanApplications { get; set; } = new List<LoanApplicationViewModel>();
        
        [Inject] public required NavigationManager NavigationManager { get; set; }



        [Inject] public required IPortalService PortalService { get; set; }
        [Inject] public required HelperService HelperService { get; set; }
        [Inject] public AuthService? AuthService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                HelperService.SetIsLoadingState(true);

                Workers = await PortalService.Worker.GetAllWorkersLogsAsync();

                DashboardStatistics01 = await PortalService.Statistics.GetDashboardStatistics01Async();

                LoanApplications = (await PortalService.Client.GetAllLoanApplicationsAsnyc()).Where(x => x.LoanRepaymentStatus == LoanRepaymentStatus.Current);

                HelperService.SetIsLoadingState(false);
            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex);
            }
        }


        public void RowClicked(object args)
        {
            var record = (LoanApplicationViewModel)args;

            NavigationManager.NavigateTo($"/loan/details/{record.Id}", true);
        }

        public void RowRender(RowRenderEventArgs<LoanApplicationViewModel> args)
        {
            try
            {
                if (args.Data.LoanRepaymentStatus == LoanRepaymentStatus.Completed) args.Attributes.Add("Style", $"background-color:{Library.Constants.Colors.Green};");
            }
            catch (Exception)
            {
                //do nothing
            }

        }

        public async Task ExecuteLoanInterestMonitor()
        {
            try
            {
                HelperService.SetIsLoadingState(true);

                string result = await PortalService.Loan.ExecuteLoanInterestMonitorAsync();

                HelperService.SetSuccessMessage(result);
            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex);
            }
        }
        public async Task ExecuteLoanStatusMonitor()
        {
            try
            {
                HelperService.SetIsLoadingState(true);

                string result = await PortalService.Loan.ExecuteLoanStatusMonitorAsync();

                HelperService.SetSuccessMessage(result);
            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex);
            }
        }
        public async Task ExecuteLoanReconBatchMonitor()
        {
            try
            {
                HelperService.SetIsLoadingState(true);

                string result = await PortalService.Batch.ExecuteLoanReconBatchMonitorAsync();

                HelperService.SetSuccessMessage(result);
            }
            catch (Exception ex)
            {
                HelperService.SetErrorMessage(ex);
            }
        }
    }
}
