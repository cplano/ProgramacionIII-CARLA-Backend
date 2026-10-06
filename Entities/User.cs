using System.ComponentModel.DataAnnotations; // Para los atributos de validacion ([Required], [EmailAddress]...)

namespace Entities
{
    // Usuario del sistema (el que inicia sesion).
    // Hereda de Person: ya tiene Id, Name, Age y Dni.
    public class User : Person
    {
        // ATRIBUTOS DE VALIDACION: instrucciones que se ponen arriba de una propiedad.
        // [Required] = obligatorio. [StringLength(100)] = maximo 100 caracteres
        // (ademas EF crea la columna como varchar(100) en vez de longtext).

        [Required]
        [StringLength(100)]
        public string Lastname { get; set; } = string.Empty; // Apellido

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty; // Nombre de usuario

        // [EmailAddress] = tiene que tener formato de correo (con @).
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        // NUNCA se guarda la contraseña real: se guarda su HASH (generado con BCrypt).
        // El hash no se puede "desencriptar"; para el login se compara con BCrypt.Verify.
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
