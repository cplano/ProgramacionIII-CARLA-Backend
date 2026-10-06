namespace API.DTOs
{
    // Lo que devuelve la API cuando el login sale bien:
    //   { "token": "eyJhbGciOi...", "user": { "id": 1, "fullname": "...", ... } }
    public class LoginResponseDTO
    {
        // Token JWT: el cliente lo guarda y lo manda en los siguientes pedidos
        // para demostrar que ya inicio sesion.
        public string Token { get; set; } = string.Empty;

        public UserResponseDTO User { get; set; } = new UserResponseDTO();
    }
}
