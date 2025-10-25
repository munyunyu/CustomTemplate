using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Authentication;
using System.Security.Claims;

namespace MVA.Portal.Services
{
    public class AuthService : ComponentBase
    {
        public AuthenticationStateProvider AuthenticationState { get; set; }

        public AuthService(AuthenticationStateProvider _authenticationState)
        {
            AuthenticationState = _authenticationState;
        }

        public async Task<string> GetCurrentUserIdAsync()
        {
            try
            {
                var authstate = await AuthenticationState.GetAuthenticationStateAsync();

                var authdata = authstate?.User?.Identity ?? throw new Exception("Auth identity is null");

                var userId = ((ClaimsIdentity)authdata)?.FindFirst("UserId")?.Value;

                if (userId == null) throw new Exception("Auth identity: userId was not found");

                return userId;
            }
            catch (Exception)
            {
                throw new AuthenticationException("User claims were not found, please re-login and try again");
            }
        }

        public async Task<string> GetCurrentUserToken()
        {
            try
            {
                var authstate = await AuthenticationState.GetAuthenticationStateAsync();

                var authdata = authstate?.User?.Identity ?? throw new Exception("Auth identity is null");

                var token = ((ClaimsIdentity)authdata)?.FindFirst("Token")?.Value;

                if (token == null) throw new Exception("Auth identity: token was not found");

                return token;
            }
            catch (Exception)
            {
                throw new AuthenticationException("User claims were not found, please re-login and try again");
            }
        }
    }
}
