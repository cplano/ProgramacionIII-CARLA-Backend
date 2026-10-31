using Microsoft.AspNetCore.Mvc; // Para ControllerBase, [ApiController], [HttpGet], Ok(), NotFound(), etc.
using Entities;                 // Para usar la clase Activity
using DAO;                      // Para usar ActivityDAO

namespace API.Controllers
{
    // Controller de actividades. Misma estructura que PlayerController (ver comentarios alli).
    // URL base: /api/Activity
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly ActivityDAO _activityDAO;

        // ASP.NET inyecta el ActivityDAO registrado en Program.cs.
        public ActivityController(ActivityDAO activityDAO)
        {
            _activityDAO = activityDAO;
        }

        // GET /api/Activity -> todas las actividades.
        [HttpGet]
        public ActionResult<List<Activity>> GetAll()
        {
            return Ok(_activityDAO.GetAll()); // 200 OK
        }

        // GET /api/Activity/1 -> una actividad por Id.
        [HttpGet("{id}")]
        public ActionResult<Activity> GetById(long id)
        {
            var activity = _activityDAO.GetById(id);
            if (activity == null) return NotFound(); // 404 Not Found
            return Ok(activity); // 200 OK
        }

        // POST /api/Activity -> crea una actividad (JSON en el cuerpo del pedido).
        // La descripcion es opcional; el titulo es obligatorio.
        [HttpPost]
        public ActionResult<Activity> Create([FromBody] Activity activity)
        {
            if (string.IsNullOrEmpty(activity.Title))
            {
                return BadRequest("El titulo de la actividad es obligatorio."); // 400 Bad Request
            }

            var createdActivity = _activityDAO.Create(activity);
            return Created($"/api/activity/{createdActivity.Id}", createdActivity); // 201 Created
        }

        // PUT /api/Activity/1 -> modifica una actividad.
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Activity activity)
        {
            bool updated = _activityDAO.Update(id, activity);
            if (!updated) return NotFound(); // 404 Not Found

            return Ok(activity); // 200 OK
        }

        // DELETE /api/Activity/1 -> borra una actividad.
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            bool deleted = _activityDAO.Delete(id);
            if (!deleted) return NotFound(); // 404 Not Found

            return NoContent(); // 204 No Content
        }
    }
}
