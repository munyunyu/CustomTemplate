using Microsoft.AspNetCore.Components;
using MVA.Business.Interface;
using MVA.Portal.Services;

namespace MVA.Portal.Pages.Helper
{
    public partial class BasePage
    {
        [Inject] public required IPortalService PortalService { get; set; }
        [Inject] public required HelperService HelperService { get; set; }
        [Inject] public required AuthService? AuthService { get; set; }
        [Inject] public required NavigationManager NavigationManager { get; set; }
    }
}
