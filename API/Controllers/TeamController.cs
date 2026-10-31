using Microsoft.AspNetCore.Mvc; // Para ControllerBase, [ApiController], [HttpGet], Ok(), NotFound(), etc.
using Entities;                 // Para usar la clase Team
using DAO;                      // Para usar TeamDAO

namespace API.Controllers
{
    // Controller de equipos. Misma estructura que PlayerController (ver comentarios alli).
    // URL base: /api/Team
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly TeamDAO _teamDAO;

        // ASP.NET inyecta el TeamDAO registrado en Program.cs.
        public TeamController(TeamDAO teamDAO)
        {
            _teamDAO = teamDAO;
        }

        // GET /api/Team -> todos los equipos.
        [HttpGet]
        public ActionResult<List<Team>> GetAll()
        {
            return Ok(_teamDAO.GetAll()); // 200 OK
        }

        // GET /api/Team/1 -> un equipo por Id.
        [HttpGet("{id}")]
        public ActionResult<Team> GetById(long id)
        {
            var team = _teamDAO.GetById(id);
            if (team == null) return NotFound(); // 404 Not Found
            return Ok(team); // 200 OK
        }

        // POST /api/Team -> crea un equipo (JSON en el cuerpo del pedido).
        [HttpPost]
        public ActionResult<Team> Create([FromBody] Team team)
        {
            if (string.IsNullOrEmpty(team.Name) || string.IsNullOrEmpty(team.Category))
            {
                return BadRequest("El nombre y la categoria del equipo son obligatorios."); // 400 Bad Request
            }

            var createdTeam = _teamDAO.Create(team);
            return Created($"/api/team/{createdTeam.Id}", createdTeam); // 201 Created
        }

        // PUT /api/Team/1 -> modifica un equipo.
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Team team)
        {
            bool updated = _teamDAO.Update(id, team);
            if (!updated) return NotFound(); // 404 Not Found

            return Ok(team); // 200 OK
        }

        // DELETE /api/Team/1 -> borra un equipo.
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            bool deleted = _teamDAO.Delete(id);
            if (!deleted) return NotFound(); // 404 Not Found

            return NoContent(); // 204 No Content
        }
    }
}
