using Microsoft.AspNetCore.Mvc;
using Entities;
using DAO;

namespace API.Controllers
{
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
            return Ok(_teamDAO.GetAll());
        }

        // GET /api/Team/1 -> un equipo por Id.
        [HttpGet("{id}")]
        public ActionResult<Team> GetById(long id)
        {
            var team = _teamDAO.GetById(id);
            if (team == null) return NotFound();
            return Ok(team);
        }

        [HttpPost]
        public ActionResult<Team> Create([FromBody] Team team)
        {
            if (string.IsNullOrEmpty(team.Name) || string.IsNullOrEmpty(team.Category))
            {
                return BadRequest("El nombre y la categoria del equipo son obligatorios.");
            }

            var createdTeam = _teamDAO.Create(team);
            return Created($"/api/team/{createdTeam.Id}", createdTeam);
        }

        // PUT /api/Team/1 -> modifica un equipo.
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Team team)
        {
            bool updated = _teamDAO.Update(id, team);
            if (!updated) return NotFound();

            return Ok(team);
        }

        // DELETE /api/Team/1 -> borra un equipo.
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            bool deleted = _teamDAO.Delete(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
