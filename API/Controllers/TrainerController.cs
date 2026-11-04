using Microsoft.AspNetCore.Mvc;
using Entities;
using DAO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerController : ControllerBase
    {
        private readonly TrainerDAO _trainerDAO;

        // ASP.NET inyecta el TrainerDAO registrado en Program.cs.
        public TrainerController(TrainerDAO trainerDAO)
        {
            _trainerDAO = trainerDAO;
        }

        // GET /api/Trainer -> todos los entrenadores.
        [HttpGet]
        public ActionResult<List<Trainer>> GetAll()
        {
            return Ok(_trainerDAO.GetAll());
        }

        // GET /api/Trainer/1 -> un entrenador por Id.
        [HttpGet("{id}")]
        public ActionResult<Trainer> GetById(long id)
        {
            var trainer = _trainerDAO.GetById(id);
            if (trainer == null) return NotFound();
            return Ok(trainer);
        }

        [HttpPost]
        public ActionResult<Trainer> Create([FromBody] Trainer trainer)
        {
            if (string.IsNullOrEmpty(trainer.Name))
            {
                return BadRequest("El nombre del entrenador es obligatorio.");
            }

            var createdTrainer = _trainerDAO.Create(trainer);
            return Created($"/api/trainer/{createdTrainer.Id}", createdTrainer);
        }

        // PUT /api/Trainer/1 -> modifica un entrenador.
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Trainer trainer)
        {
            bool updated = _trainerDAO.Update(id, trainer);
            if (!updated) return NotFound();

            return Ok(trainer);
        }

        // DELETE /api/Trainer/1 -> borra un entrenador.
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            bool deleted = _trainerDAO.Delete(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
