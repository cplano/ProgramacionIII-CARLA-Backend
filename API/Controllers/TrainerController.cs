using Microsoft.AspNetCore.Mvc; // Para ControllerBase, [ApiController], [HttpGet], Ok(), NotFound(), etc.
using Entities;                 // Para usar la clase Trainer
using DAO;                      // Para usar TrainerDAO

namespace API.Controllers
{
    // Controller de entrenadores. Misma estructura que PlayerController (ver comentarios alli).
    // URL base: /api/Trainer
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
            return Ok(_trainerDAO.GetAll()); // 200 OK
        }

        // GET /api/Trainer/1 -> un entrenador por Id.
        [HttpGet("{id}")]
        public ActionResult<Trainer> GetById(long id)
        {
            var trainer = _trainerDAO.GetById(id);
            if (trainer == null) return NotFound(); // 404 Not Found
            return Ok(trainer); // 200 OK
        }

        // POST /api/Trainer -> crea un entrenador (JSON en el cuerpo del pedido).
        [HttpPost]
        public ActionResult<Trainer> Create([FromBody] Trainer trainer)
        {
            if (string.IsNullOrEmpty(trainer.Name))
            {
                return BadRequest("El nombre del entrenador es obligatorio."); // 400 Bad Request
            }

            var createdTrainer = _trainerDAO.Create(trainer);
            return Created($"/api/trainer/{createdTrainer.Id}", createdTrainer); // 201 Created
        }

        // PUT /api/Trainer/1 -> modifica un entrenador.
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Trainer trainer)
        {
            bool updated = _trainerDAO.Update(id, trainer);
            if (!updated) return NotFound(); // 404 Not Found

            return Ok(trainer); // 200 OK
        }

        // DELETE /api/Trainer/1 -> borra un entrenador.
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            bool deleted = _trainerDAO.Delete(id);
            if (!deleted) return NotFound(); // 404 Not Found

            return NoContent(); // 204 No Content
        }
    }
}
