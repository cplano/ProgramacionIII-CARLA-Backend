using System.IdentityModel.Tokens.Jwt; // JwtSecurityToken y JwtSecurityTokenHandler
using System.Security.Claims;          // Claim: cada dato que viaja dentro del token
using System.Text;
using Entities;
using Microsoft.IdentityModel.Tokens;  // SymmetricSecurityKey y SigningCredentials

namespace API.Services
{
    // Genera los TOKENS JWT (JSON Web Token).
    // Un JWT es un texto con 3 partes separadas por puntos: HEADER.PAYLOAD.FIRMA
    //  - Header: el algoritmo usado (HS256).
    //  - Payload: los datos (claims) del usuario: id, email, vencimiento...
    //    OJO: el payload NO esta encriptado, cualquiera lo puede leer (ej: en jwt.io).
    //    Por eso nunca se pone la contraseña adentro.
    //  - Firma: se calcula con la clave secreta del servidor. Si alguien modifica
    //    el payload, la firma deja de coincidir y la API rechaza el token.
    public class TokenService
    {
        // IConfiguration permite leer appsettings.json / appsettings.Development.json
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
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // "subject": el id del usuario
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("username", user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // id unico del token
            };

            // FIRMA: la clave secreta (seccion "Jwt" del appsettings) se convierte en bytes.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            // HmacSha256 = algoritmo HS256 para firmar.
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            int minutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],     // Quien emite el token (nuestra API)
                audience: _configuration["Jwt:Audience"], // Para quien es (nuestro frontend)
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutes), // Vencimiento
                signingCredentials: credentials
            );

            // Convierte el objeto token en el texto "xxxxx.yyyyy.zzzzz"
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
