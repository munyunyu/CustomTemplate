using System;
using System.Collections.Generic;
using System.Text;
using Template.Library.ViewsModels.System;

namespace Template.Library.Extensions
{
    public static class ProfileInitialsExtensions
    {
        public static string GetProfileInitials(this ApplicationUserViewModel? user)
        {
            if (user == null) return "APP";

            // 1️⃣ Use first + last name if available
            if (!string.IsNullOrWhiteSpace(user.FirstName) || !string.IsNullOrWhiteSpace(user.LastName))
            {
                var firstInitial = !string.IsNullOrWhiteSpace(user.FirstName) ? user.FirstName.Trim()[0].ToString() : string.Empty;

                var lastInitial = !string.IsNullOrWhiteSpace(user.LastName) ? user.LastName.Trim()[0].ToString() : string.Empty;

                return (firstInitial + lastInitial).ToUpperInvariant();
            }

            // 2️⃣ Fallback to email
            if (!string.IsNullOrWhiteSpace(user.Email)) return user.Email.Trim()[0].ToString().ToUpperInvariant();

            // 3️⃣ Final fallback
            return "?";
        }
    }

}
