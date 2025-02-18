using Domain.Enums;
using Domain.Models.Organization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Providers
{
    public static class AuthTokensProvider
    {
        public static readonly string ISSUER = "NatureHelpIssuer";
        public static readonly string AUDIENCE = "WeLikeNature";
        private const string KEY = "nat9631natureHelp2002securiTYKEy!forJwts";
        public static SymmetricSecurityKey GetSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));

        public static string GenerateAccessToken(User user)
        {
            var claims = GetClaimsFromUser(user);

            var jwt = new JwtSecurityToken(
                issuer: ISSUER,
                audience: AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(0.5)),
                signingCredentials: new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public static string GenerateRefreshToken(User user)
        {
            var claims = GetClaimsFromUser(user);

            var jwt = new JwtSecurityToken(
                issuer: ISSUER,
                audience: AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(3)),
                signingCredentials: new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private static List<Claim> GetClaimsFromUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var roleString = user.Role.ToString();

            return claims;
        }
    }
}
