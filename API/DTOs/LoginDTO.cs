using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    // DTO (Data Transfer Object = objeto para transferir datos).
    // Es un "contrato": define EXACTAMENTE lo que la API espera RECIBIR en el login.
    // El cliente (Angular, Swagger, Postman) manda:
    //   { "email": "daniel@email.com", "password": "123456" }
    // y ASP.NET lo convierte automaticamente en un objeto LoginDTO.
    public class LoginDTO
    {
        // Si falta o no tiene formato de mail, [ApiController] responde 400 Bad Request
        // automaticamente, sin llegar a ejecutar el metodo del controller.
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato valido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; } = string.Empty;
    }
}
