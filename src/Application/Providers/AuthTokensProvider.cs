using Domain.Models.Organization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

        public static bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(token))
            {
                throw new InvalidOperationException("The token can not be read...");
            }

            var jwtToken = tokenHandler.ReadJwtToken(token);
            var expiration = jwtToken.ValidTo;

            return expiration < DateTime.UtcNow;
        }

        private static List<Claim> GetClaimsFromUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            return claims;
        }

        public static string GenerateEmailConfirmationToken(int length = 32)
        {
            var bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes)
                         .Replace("+", "-")
                         .Replace("/", "_")
                         .Replace("=", "");
        }

        public static string? ExtractAccessTokenFromRequest(HttpRequest request)
        {
            string? authorizationHeader = request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader)) return null;

            string token = authorizationHeader.StartsWith("Bearer ")
                ? authorizationHeader["Bearer ".Length..]
                : authorizationHeader;

            return token;
        }
    }
}
