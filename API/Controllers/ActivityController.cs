using Microsoft.AspNetCore.Mvc;
using Entities;
using DAO;

namespace API.Controllers
{
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
            return Ok(_activityDAO.GetAll());
        }

        // GET /api/Activity/1 -> una actividad por Id.
        [HttpGet("{id}")]
        public ActionResult<Activity> GetById(long id)
        {
            var activity = _activityDAO.GetById(id);
            if (activity == null) return NotFound();
            return Ok(activity);
        }

        [HttpPost]
        public ActionResult<Activity> Create([FromBody] Activity activity)
        {
            if (string.IsNullOrEmpty(activity.Title))
            {
                return BadRequest("El titulo de la actividad es obligatorio.");
            }

            var createdActivity = _activityDAO.Create(activity);
            return Created($"/api/activity/{createdActivity.Id}", createdActivity);
        }

        // PUT /api/Activity/1 -> modifica una actividad.
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Activity activity)
        {
            bool updated = _activityDAO.Update(id, activity);
            if (!updated) return NotFound();

            return Ok(activity);
        }

        // DELETE /api/Activity/1 -> borra una actividad.
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            bool deleted = _activityDAO.Delete(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
