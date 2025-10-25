using MVA.Library.Models.Client;
using MVA.Portal.Models.Client;

namespace MVA.Portal.Extensions
{
    public static class LoanExtensions
    {
        public static List<ClientLoanApplicationSummaryModel> GetClientLoanApplicationSummaries(this ClientViewModel client)
        {
            List<ClientLoanApplicationSummaryModel> clientLoans = new List<ClientLoanApplicationSummaryModel>();

            if (client.LoanApplications == null) return clientLoans;

            foreach (var loan in client.LoanApplications)
            {
                var amountPaid = loan?.LoanRepayments?.Sum(x => x.Amount) ?? 0;

                var transactions = loan?.LoanRepayments?.Count ?? 0;

                var model = new ClientLoanApplicationSummaryModel
                {
                    Id = loan?.Id,
                    ClientId = loan?.ClientId,
                    CreatedDate = loan?.CreatedDate.ToString("yyyy-MM-dd"),
                    LoanAmount = $"{loan?.Currency} {loan?.Amount}",
                    AmountPaid = $"{loan?.Currency} {amountPaid}",
                    TotalTransactions = $"{transactions} transactions",
                    LoanApplicationStatus = loan.LoanApplicationStatus,
                    LoanRepaymentStatus = loan.LoanRepaymentStatus,
                    LoanDisbursalMethod = (loan?.LoanDisbursal?.LoanDisbursalMethod != null) ? "Cash" : "Pending Disbursal",
                    DisbursalActionedDate = (loan?.LoanDisbursal?.ActionedDate != null) ? ((DateTime)(loan.LoanDisbursal.ActionedDate)).ToString("yyyy-MM-dd") : "N/A",                   
                };

                clientLoans.Add(model);
            }

            return clientLoans;
        }

        
    }
}
