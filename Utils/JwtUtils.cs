using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Muuki.Models;

namespace Muuki.Utils
{
    public class JwtUtils
    {
        private readonly IConfiguration _config;

        public JwtUtils(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            var secret = Environment.GetEnvironmentVariable("JWT_SECRET");
            if (string.IsNullOrEmpty(secret))
            {
                throw new Exception("La clave JWT no está configurada en las variables de entorno.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] 
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}