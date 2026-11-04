using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Entities;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            // CLAIMS: datos del usuario que viajan dentro del token.
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("username", user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            // HmacSha256 = algoritmo HS256 para firmar.
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            int minutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutes),
                signingCredentials: credentials
            );

            // Convierte el objeto token en el texto "xxxxx.yyyyy.zzzzz"
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
