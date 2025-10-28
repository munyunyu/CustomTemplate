using Microsoft.AspNetCore.Components;
using Template.Portal.Interface;
using Template.Portal.Services.System;

namespace Template.Portal.Pages.Helper
{
    public partial class BasePage
    {
        [Inject] public required IPortalService PortalService { get; set; }
        [Inject] public required HelperService HelperService { get; set; }
        [Inject] public required AuthService AuthService { get; set; }
        [Inject] public required NavigationManager NavigationManager { get; set; }
    }
}
