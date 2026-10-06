namespace API.DTOs
{
    // DTO de RESPUESTA: define lo que la API esta dispuesta a DEVOLVER de un usuario.
    // NO incluye PasswordHash: el hash nunca sale del servidor (seguridad).
    public class UserResponseDTO
    {
        public long Id { get; set; }
        public string Fullname { get; set; } = string.Empty; // Nombre + apellido juntos
        public string Dni { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}
