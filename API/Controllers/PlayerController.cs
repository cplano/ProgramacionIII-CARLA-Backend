using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAO; // Se agrego este using
using Entities; // Se agrego este using
using System.Collections.Generic; // Se agrego este using


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerDAO _playerDAO;

        public PlayerController(PlayerDAO playerDAO)
        {
            _playerDAO = playerDAO;
        }

        [HttpGet]
        public ActionResult<List<Player>> GetAll()
        {
            return Ok(_playerDAO.GetAll()); // 200 OK
        }

        [HttpGet("{id}")]
        public ActionResult<Player> GetById(long id)
        {
            var player = _playerDAO.GetById(id);
            if (player == null) return NotFound(); // 404 Not Found
            return Ok(player); // 200 OK
        }

        [HttpPost]
        public ActionResult<Player> Create([FromBody] Player player)
        {
            if (string.IsNullOrEmpty(player.Name))
            {
                return BadRequest("El nombre del jugador es requerido."); // 400 Bad Request
            }

            var createdPlayer = _playerDAO.Create(player);
            return Created($"/api/player/{createdPlayer.Id}", createdPlayer); // 201 Created
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Player player)
        {
            bool updated = _playerDAO.Update(id, player);
            if (!updated) return NotFound(); // 404 Not Found

            return Ok(player); // 200 OK
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            bool deleted = _playerDAO.Delete(id);
            if (!deleted) return NotFound(); // 404 Not Found

            return NoContent(); // 204 No Content
        }
    }
}
