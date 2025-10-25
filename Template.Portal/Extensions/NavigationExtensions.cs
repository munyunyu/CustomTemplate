using Microsoft.AspNetCore.Components;

namespace MVA.Portal.Extensions
{
    public static class NavigationExtensions
    {
        public static void ReloadPage(this NavigationManager manager)
        {
            manager.NavigateTo(manager.Uri, true);
        }
    }
}
