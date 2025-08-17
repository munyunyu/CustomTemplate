using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Template.Database.Context;

namespace Template.Service.Extensions
{
    public static class JwtTokenExtensions
    {
        public static JwtSecurityToken GenerateJwtSecurityToken(this Claim[] claims, IConfiguration configuration)
        {
            string securityKey = configuration["Jwt:SigningKey"] ?? string.Empty;

            int expiryInMinutes = Convert.ToInt32(configuration["Jwt:ExpiryInMinutes"]);

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            var signingCredetials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Site"],
                audience: configuration["Jwt:Site"],
                expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                signingCredentials: signingCredetials,
                claims: claims
                );

            return token;
        }
        public static Claim[] GenerateUserClaims(this ApplicationUser user, List<string> roles, List<Claim> user_claims)
        {
            var claim = new List<Claim> {
                    new Claim("UserId", user.Id),  
                    new Claim(ClaimTypes.Name, user.Email ?? ""),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                };


            if(roles != null)
                if (roles.Count > 0)
                    foreach (var item in roles)
                        claim.Add(new Claim(ClaimTypes.Role, item));

            if (user_claims != null)
                if (user_claims.Count > 0)
                    foreach (var item in user_claims)
                        claim.Add(new Claim(item.Type, item.Value));

            return claim.ToArray();
        }

        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null) throw new Exception("Login claims not found for this user");

            //var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            var claim = principal.FindFirst("UserId");

            if (claim == null) throw new Exception("Claim: UserId NameIdentifier was not found");

            return Guid.Parse(claim.Value);
        }  
        
        public static Guid GetUserProfileId(this ClaimsPrincipal principal)
        {
            if (principal == null) throw new Exception("Login claims not found for this user");

            //var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            var claim = principal.FindFirst("ProfileId");

            if (claim == null) throw new Exception($"Claim: ProfileId NameIdentifier was not found");

            return Guid.Parse(claim.Value);
        }
    }
}
