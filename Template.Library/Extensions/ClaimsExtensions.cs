using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Template.Library.Models;

namespace Template.Library.Extensions
{
    public static class ClaimsExtensions
    {
        public static Claim[] GetUserClaims(this ResponseLoginAccount? model)
        {
            if (model == null) return Array.Empty<Claim>();

            var claims = new List<Claim>
                {
                  new Claim(ClaimTypes.Name, model.Email ?? string.Empty),
                  new Claim("UserId",model.UserId ?? string.Empty),
                  new Claim("Token", model.Token ?? string.Empty),
                  new Claim("Expiration", model.Expiration?.ToString() ?? string.Empty)
            };

            if(model.Roles != null)
                foreach (var role in model.Roles) 
                { 
                    var _claim = new Claim(ClaimTypes.Role, role); 
                    claims.Add(_claim); 
                }

            if (model.Claims != null)
                foreach (var claim in model.Claims) 
                { 
                    var _claim = new Claim(ClaimTypes.Role, claim);
                    claims.Add(_claim); 
                }

            return claims.ToArray();
        }

        public static List<string> GetConstantValues<T>(this T entity) where T : class
        {
            // Get the type of the class
            Type type = typeof(T);

            // Get all fields of the class
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                                      .Where(f => f.IsLiteral && !f.IsInitOnly) // Filter for constants
                                      .ToArray();

            // Retrieve the values and convert them to a list of strings
            List<string?> values = fields.Select(f => f.GetValue(null)?.ToString()).ToList();

            // Convert List<string?> to List<string>
            List<string> nonNullableList = values
                .Where(s => s != null)  // Filter out null values
                .Select(s => s!)        // Use the null-forgiving operator to cast to non-nullable
                .ToList();

            return nonNullableList;
        }
    }
}
