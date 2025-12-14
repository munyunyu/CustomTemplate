using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Template.Portal.Components.Shared.Helpers;
using Template.Portal.Services;

namespace Template.Portal.Components.Pages.Account.Login
{
    public partial class Login : BasePage
    {
        protected override async Task OnInitializedAsync()
        {
           

            var user = await PortalService.Account.LoginUserUserAsync(model: new Library.Models.RequestLoginAccount
            {
                 Email = "percy.munyunyu@gmail.com",
                 Password = "tc#Prog219!"
            });

            //var claims = user.GetUserClaims();

            //var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        }
    }
}
