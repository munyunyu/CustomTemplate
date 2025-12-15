using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Claims;
using System.Security.Principal;
using Template.Library.Extensions;
using Template.Library.Models;
using Template.Portal.Components.Shared.Helpers;
using Template.Portal.Services;
using Template.Portal.Services.System;
using static System.Net.WebRequestMethods;

namespace Template.Portal.Components.Pages.Account.Login
{
    public partial class Login : BasePage
    {
        public RequestLoginAccount Model { get; set; } = new RequestLoginAccount();
        [Inject] public required IHttpContextAccessor _httpContextAccessor { get; set; }
        //protected override async Task OnInitializedAsync()
        //{

        //}
        public async Task HandleValidUserLoginSubmit()
        {
            try
            {

                //var response = await Http.PostAsJsonAsync("/login", Model);




                var user = await PortalService.Account.LoginUserUserAsync(model: Model);

                var claims = user.GetUserClaims();

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                if (_httpContextAccessor.HttpContext != null)
                {
                    // Use InvokeAsync to execute on the synchronization context
                    await InvokeAsync(async () =>
                    {
                        await _httpContextAccessor.HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            principal,
                            new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                            });
                    });
                }

                NavigationManager.NavigateTo("/", forceLoad: true);
            }
            catch (Exception ex)
            {
                //HelperService.SetErrorMessage(ex);
            }
        }

        private void HandleInvalidSubmit(EditContext context)
        {
            Console.WriteLine("FORM IS INVALID");
        }
    }
}
