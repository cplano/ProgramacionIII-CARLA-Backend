using API.DTOs;                           // LoginDTO, RegisterDTO, UserResponseDTO, LoginResponseDTO
using API.Helpers;                        // PasswordHasher (BCrypt)
using API.Services;                       // TokenService (JWT)
using DAO;                                // UserDAO
using Entities;                           // User
using Microsoft.AspNetCore.Authorization; // [Authorize]
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Controller de usuarios: registro, login y consulta.
    // URL base: /api/User
    [Route("api/[controller]")]
    [ApiController] // Valida solo los DTOs: si un [Required] falla, responde 400 sin entrar al metodo
    public class UserController : ControllerBase
    {
        private readonly UserDAO _userDAO;
        private readonly TokenService _tokenService;

        // ASP.NET inyecta el DAO y el servicio de tokens (registrados en Program.cs).
        public UserController(UserDAO userDAO, TokenService tokenService)
        {
            _userDAO = userDAO;
            _tokenService = tokenService;
        }

        // POST /api/User/register -> crea un usuario nuevo.
        // Recibe un RegisterDTO (con la contraseña en texto plano).
        [HttpPost("register")]
        public ActionResult<UserResponseDTO> Register([FromBody] RegisterDTO dto)
        {
            // REGLA DE NEGOCIO (validacion manual): el email no se puede repetir.
            // No la puede hacer un atributo porque hay que consultar la base de datos.
            if (_userDAO.ExistsEmail(dto.Email))
            {
                return Conflict("Ya existe un usuario con ese email."); // 409 Conflict
            }

            // Se pasa del DTO a la entidad. La contraseña se guarda HASHEADA, nunca en texto plano.
            var user = new User
            {
                Name = dto.Name,
                Lastname = dto.Lastname,
                Age = dto.Age,
                Dni = dto.Dni,
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = PasswordHasher.HashPassword(dto.Password)
            };

            var created = _userDAO.Create(user);

            // Se devuelve un DTO de respuesta (sin el hash), no la entidad.
            return Created($"/api/user/{created.Id}", ToResponse(created)); // 201 Created
        }

        // POST /api/User/login -> inicia sesion.
        // Recibe: { "email": "...", "password": "..." }
        // Devuelve: 200 con el token si es correcto, 401 si no.
        [HttpPost("login")]
        public ActionResult<LoginResponseDTO> Login([FromBody] LoginDTO dto)
        {
            // 1) El DAO busca el usuario por email en la base (Entity Framework -> MySQL).
            var user = _userDAO.GetByEmail(dto.Email);

            // 2) BCrypt compara la contraseña escrita con el hash guardado.
            //    Si el usuario no existe O la contraseña no coincide, se responde lo MISMO:
            //    asi no se le revela a un atacante si el email esta registrado o no.
            if (user == null || !PasswordHasher.VerifyPassword(dto.Password, user.PasswordHash))
            {
                return Unauthorized("Email o contraseña incorrectos."); // 401 Unauthorized
            }

            // 3) Credenciales correctas: se genera el token JWT firmado.
            var response = new LoginResponseDTO
            {
                Token = _tokenService.GenerateToken(user),
                User = ToResponse(user)
            };

            return Ok(response); // 200 OK
        }

        // GET /api/User -> lista de usuarios.
        // [Authorize]: solo se puede usar enviando un token valido en el encabezado
        //   Authorization: Bearer <token>
        // Sin token (o con uno vencido/modificado) responde 401 automaticamente.
        [Authorize]
        [HttpGet]
        public ActionResult<List<UserResponseDTO>> GetAll()
        {
            // Select transforma cada User en un UserResponseDTO.
            var users = _userDAO.GetAll().Select(ToResponse).ToList();
            return Ok(users); // 200 OK
        }

        // GET /api/User/5 -> un usuario por Id (tambien requiere token).
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<UserResponseDTO> GetById(long id)
        {
            var user = _userDAO.GetById(id);
            if (user == null) return NotFound(); // 404 Not Found
            return Ok(ToResponse(user));         // 200 OK
        }

        // Metodo auxiliar privado: convierte la entidad User en el DTO de respuesta.
        // "static" porque no usa ningun dato del controller.
        private static UserResponseDTO ToResponse(User user)
        {
            return new UserResponseDTO
            {
                Id = user.Id,
                Fullname = $"{user.Name} {user.Lastname}",
                Dni = user.Dni,
                Email = user.Email,
                Username = user.Username
            };
        }
    }
}
