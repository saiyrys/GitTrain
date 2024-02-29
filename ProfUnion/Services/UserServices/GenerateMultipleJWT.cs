using Microsoft.IdentityModel.Tokens;
using Profunion.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Profunion.Services.UserServices
{

    public class GenerateMultipleJWT
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GenerateMultipleJWT(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public string GenerateAccessToken(User user)
        {
            var tokenSettings = _configuration.GetSection("TokenSettings");
            var keySizeInBits = 256;

            SigningCredentials signingCredentials;

            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                var keyBytes = new byte[keySizeInBits / 8];
                rng.GetBytes(keyBytes);

                var secretKey = new SymmetricSecurityKey(keyBytes);
                signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(tokenSettings.GetValue<int>("AccessTokenExpiration")); // Use the configured expiration time

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = now,
                Expires = expires,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Serialize the token using System.Text.Json
            var tokenJson = tokenHandler.WriteToken(token);

            return tokenJson;
        }
        public string GenerateRefreshToken()
        {
            // Generate a secure refresh token (you might want to use a library for this)
            // Save this token in your database or some secure storage for later validation

            string refreshToken = GenerateSecureToken();

            SetCookieRefresh(refreshToken);

            return refreshToken;
        }

        private string GenerateSecureToken()
        {
            byte[] tokenBytes = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }
            return Convert.ToBase64String(tokenBytes);
        }
        private void SetCookieRefresh(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                SameSite = SameSiteMode.None,
                Secure = true,
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
