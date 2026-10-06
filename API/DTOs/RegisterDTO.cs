using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    // DTO para REGISTRAR un usuario nuevo.
    // Recibe la contraseña en texto plano; el controller la convierte en hash
    // antes de guardarla. Por eso no se recibe directamente la entidad User
    // (que tiene PasswordHash en vez de Password).
    public class RegisterDTO
    {
        // StringLength con MinimumLength: entre 3 y 100 caracteres.
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, MinimumLength = 2)]
        public string Lastname { get; set; } = string.Empty;

        // Range: la edad tiene que estar entre 1 y 120.
        [Range(1, 120)]
        public int Age { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        public string Dni { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato valido")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; } = string.Empty;
    }
}
