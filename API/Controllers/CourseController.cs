using Microsoft.AspNetCore.Mvc; // Para ControllerBase, [ApiController], [HttpGet], Ok(), NotFound(), etc.
using Entities;                 // Para usar la clase Course
using DAO;                      // Para usar CourseDAO

namespace API.Controllers
{
    // Controller de cursos. Misma estructura que PlayerController (ver comentarios alli).
    // URL base: /api/Course
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseDAO _courseDAO;

        // ASP.NET inyecta el CourseDAO registrado en Program.cs.
        public CourseController(CourseDAO courseDAO)
        {
            _courseDAO = courseDAO;
        }

        // GET /api/Course -> todos los cursos.
        [HttpGet]
        public ActionResult<List<Course>> GetAll()
        {
            return Ok(_courseDAO.GetAll()); // 200 OK
        }

        // GET /api/Course/1 -> un curso por Id.
        [HttpGet("{id}")]
        public ActionResult<Course> GetById(long id)
        {
            var course = _courseDAO.GetById(id);
            if (course == null) return NotFound(); // 404 Not Found
            return Ok(course); // 200 OK
        }

        // POST /api/Course -> crea un curso (JSON en el cuerpo del pedido).
        [HttpPost]
        public ActionResult<Course> Create([FromBody] Course course)
        {
            if (string.IsNullOrEmpty(course.Name))
            {
                return BadRequest("El nombre del curso es obligatorio."); // 400 Bad Request
            }

            var createdCourse = _courseDAO.Create(course);
            return Created($"/api/course/{createdCourse.Id}", createdCourse); // 201 Created
        }

        // PUT /api/Course/1 -> modifica un curso.
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Course course)
        {
            bool updated = _courseDAO.Update(id, course);
            if (!updated) return NotFound(); // 404 Not Found

            return Ok(course); // 200 OK
        }

        // DELETE /api/Course/1 -> borra un curso.
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            bool deleted = _courseDAO.Delete(id);
            if (!deleted) return NotFound(); // 404 Not Found

            return NoContent(); // 204 No Content
        }
    }
}
