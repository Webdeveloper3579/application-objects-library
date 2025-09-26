using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AOL_Portal.Configuration;
using AOL_Portal.Data;
using AOL_Portal.Models;
using System.Security.Cryptography;

namespace AOL_Portal.Services
{
    public interface IJwtService
    {
        Task<AuthResponse> GenerateTokenAsync(AolApplicationUser user);
        ClaimsPrincipal ValidateToken(string token);
        string GenerateRefreshToken();
    }

    public class JwtService : IJwtService
    {
        private readonly UserManager<AolApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApiConfig _apiConfig;

        public JwtService(UserManager<AolApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _apiConfig = configuration.GetSection("ApiService").Get<ApiConfig>();
        }

        public async Task<AuthResponse> GenerateTokenAsync(AolApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.Surname}"),
                new Claim("FirstName", user.FirstName),
                new Claim("Surname", user.Surname),
                new Claim("IsSiteAdmin", user.IsSiteAdmin.ToString())
            };

            // Add roles to claims
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_apiConfig.JwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_apiConfig.JwtExpiryMinutes),
                signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var refreshToken = GenerateRefreshToken();

            return new AuthResponse
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken,
                ExpiresAt = token.ValidTo,
                User = new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Surname = user.Surname,
                    IsSiteAdmin = user.IsSiteAdmin,
                    Roles = userRoles.ToList()
                }
            };
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_apiConfig.JwtSecret);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
} 