using Microsoft.AspNetCore.Components;
using MVA.Portal.Services;

namespace MVA.Portal.Shared.Helpers
{
    /// <summary>
    /// Progress bar when system is busy
    /// </summary>
    public partial class Progressbar : IDisposable
    {
        #region services injects

        [Inject]
        public HelperService? HelperService { get; set; }

        #endregion

        protected override void OnInitialized()
        {
            if (HelperService != null) HelperService.OnChange += StateHasChanged;
        }

        public void Dispose()
        {
            if (HelperService != null) HelperService.OnChange -= StateHasChanged;
        }
    }
}
