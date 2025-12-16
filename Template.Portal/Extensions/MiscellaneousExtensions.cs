using Template.Portal.Services.System;

namespace Template.Portal.Extensions
{
    public static class MiscellaneousExtensions
    {
        public static AuthService ValidateIsNull(this AuthService? authService)
        {
            if (authService == null) throw new Exception("Auth service failed to initialize");

            return authService;
        }
    }
}
