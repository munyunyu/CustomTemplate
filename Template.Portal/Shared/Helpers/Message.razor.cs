using Microsoft.AspNetCore.Components;
using Template.Portal.Services.System;

namespace Template.Portal.Shared.Helpers
{
    /// <summary>
    /// Global message status
    /// </summary>
    public partial class Message
    {
        #region services injects

        [Inject]
        public HelperService? HelperService { get; set; }

        #endregion

        protected void CloseAlert(string input)
        {
            switch (input)
            {
                case "ErrorMessage":
                    if (HelperService != null) HelperService.ErrorMessage = string.Empty;
                    break;
                case "SuccessMessage":
                    if (HelperService != null) HelperService.SuccessMessage = string.Empty;
                    break;
                default:
                    break;
            }

            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            if (HelperService != null) HelperService.OnChange += StateHasChanged;
        }

        //public void Dispose()
        //{
        //    if (HelperService != null) HelperService.OnChange -= StateHasChanged;
        //}
    }
}
