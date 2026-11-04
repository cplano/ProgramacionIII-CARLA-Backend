using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class User : Person
    {
        [Required]
        [StringLength(100)]
        public string Lastname { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        // [EmailAddress] = tiene que tener formato de correo (con @).
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
