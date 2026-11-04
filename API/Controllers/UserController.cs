using API.DTOs;
using API.Helpers;
using API.Services;
using DAO;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDAO _userDAO;
        private readonly TokenService _tokenService;

        public UserController(UserDAO userDAO, TokenService tokenService)
        {
            _userDAO = userDAO;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public ActionResult<UserResponseDTO> Register([FromBody] RegisterDTO dto)
        {
            if (_userDAO.ExistsEmail(dto.Email))
            {
                return Conflict("Ya existe un usuario con ese email.");
            }

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

            return Created($"/api/user/{created.Id}", ToResponse(created));
        }

        [HttpPost("login")]
        public ActionResult<LoginResponseDTO> Login([FromBody] LoginDTO dto)
        {
            var user = _userDAO.GetByEmail(dto.Email);

            if (user == null || !PasswordHasher.VerifyPassword(dto.Password, user.PasswordHash))
            {
                return Unauthorized("Email o contraseña incorrectos.");
            }

            // Credenciales correctas: se genera el token JWT.
            var response = new LoginResponseDTO
            {
                Token = _tokenService.GenerateToken(user),
                User = ToResponse(user)
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public ActionResult<List<UserResponseDTO>> GetAll()
        {
            // Select transforma cada User en un UserResponseDTO.
            var users = _userDAO.GetAll().Select(ToResponse).ToList();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<UserResponseDTO> GetById(long id)
        {
            var user = _userDAO.GetById(id);
            if (user == null) return NotFound();
            return Ok(ToResponse(user));
        }

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
