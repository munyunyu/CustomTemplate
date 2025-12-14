using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Security.Principal;
using Template.Library.Extensions;
using Template.Portal.Components.Shared.Helpers;
using Template.Portal.Services;
using Template.Portal.Services.System;

namespace Template.Portal.Components.Pages.Account.Login
{
    public partial class Login : BasePage
    {
        //[Inject] public required AuthenticationStateProvider _authProvider { get; set; }
        [Inject] public required IHttpContextAccessor _httpContextAccessor { get; set; }
        protected override async Task OnInitializedAsync()
        {

            var user = await PortalService.Account.LoginUserUserAsync(model: new Library.Models.RequestLoginAccount
            {
                 Email = "percy.munyunyu@gmail.com",
                 Password = "tc#Prog219!"
            });

            var claims = user.GetUserClaims();

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

            var principal = new ClaimsPrincipal(identity);

            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            if (_httpContextAccessor.HttpContext != null)
            {
                await _httpContextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                    });
            }

            NavigationManager.NavigateTo("/", forceLoad: true);
        }
    }
}
