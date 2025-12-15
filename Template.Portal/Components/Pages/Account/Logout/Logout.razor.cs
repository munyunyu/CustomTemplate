using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Components;
//using Template.Portal.Components.Shared.Helpers;

//namespace Template.Portal.Components.Pages.Account.Logout
//{
//    public partial class Logout : BasePage
//    {
//        [Inject] public required IHttpContextAccessor _httpContextAccessor { get; set; }

//        protected override async Task OnInitializedAsync()
//        {
//            if (_httpContextAccessor == null) return;

//            _httpContextAccessor.HttpContext.Response.Cookies.Delete(
//               CookieAuthenticationDefaults.AuthenticationScheme, // Default cookie name
//               new CookieOptions
//               {
//                   HttpOnly = true,
//                   Secure = true,
//                   SameSite = SameSiteMode.Strict
//               });

//            // Clear other related cookies
//            _httpContextAccessor.HttpContext.Response.Cookies.Delete(CookieAuthenticationDefaults.AuthenticationScheme);
//            //_httpContextAccessor.HttpContext.Response.Cookies.Delete(CookieAuthenticationDefaults.AuthenticationScheme);


//            NavigationManager.NavigateTo("/account/login");
//        }
//    }
//}
