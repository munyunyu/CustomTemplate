using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Template.Library.Extensions;
using Template.Library.Models;
using Template.Portal.Interface;

namespace Template.Portal.Pages.Account.Login
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IPortalService portalService;

        public LoginModel(IPortalService portalService)
        {
            this.portalService = portalService;
        }

        [BindProperty]
        public RequestLoginAccount Input { get; set; } = new RequestLoginAccount() { };

        public async Task OnGetAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await portalService.Account.LoginUserUserAsync(model: Input);

                    var claims = user.GetUserClaims();

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return LocalRedirect("/");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                    return Page();
                }


            }

            return Page();
        }
    }
}
