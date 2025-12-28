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

        public static List<string> GetConstantValues<T>(this T entity, string role = "") where T : class
        {
            var claims = typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
            .Select(f => (string)f.GetValue(null)!)
            .ToList();

            if(string.IsNullOrEmpty(role)) return claims;

            return claims.Where(x => x.StartsWith(role)).ToList();
        }
    }
}
