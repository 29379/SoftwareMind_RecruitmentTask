using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Interfaces.ServiceInterfaces;
using HotDeskBookingSystem.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotDeskBookingSystem.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly TokenStore _tokenStore;

        public JwtTokenService(IConfiguration configuration, TokenStore tokenStore)
        {
            _configuration = configuration;
            _tokenStore = tokenStore;
        }

        public string GenerateJwtToken(AppUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User cannot be null when generating a jwt token");
            }

            var jti = Guid.NewGuid().ToString();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var roles = user.UserRoles
                .Select(ur => ur.RoleName)
                .ToList();
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            DateTime now = DateTime.Now;
            var exp = now.AddHours(1);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: now.AddHours(1),
                signingCredentials: creds
            );

            _tokenStore.AddToken(jti, exp);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool IsTokenValid(string jti)
        {
            return _tokenStore.IsTokenActive(jti);
        }

        public void RemoveToken(string jti)
        {
            _tokenStore.RemoveToken(jti);
        }

        public void ClearExpiredTokens()
        {
            _tokenStore.ClearExpiredTokens();
        }

    }
}
