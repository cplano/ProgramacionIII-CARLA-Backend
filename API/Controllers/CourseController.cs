using Microsoft.AspNetCore.Mvc;
using Entities;
using DAO;

namespace API.Controllers
{
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
            return Ok(_courseDAO.GetAll());
        }

        // GET /api/Course/1 -> un curso por Id.
        [HttpGet("{id}")]
        public ActionResult<Course> GetById(long id)
        {
            var course = _courseDAO.GetById(id);
            if (course == null) return NotFound();
            return Ok(course);
        }

        [HttpPost]
        public ActionResult<Course> Create([FromBody] Course course)
        {
            if (string.IsNullOrEmpty(course.Name))
            {
                return BadRequest("El nombre del curso es obligatorio.");
            }

            var createdCourse = _courseDAO.Create(course);
            return Created($"/api/course/{createdCourse.Id}", createdCourse);
        }

        // PUT /api/Course/1 -> modifica un curso.
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Course course)
        {
            bool updated = _courseDAO.Update(id, course);
            if (!updated) return NotFound();

            return Ok(course);
        }

        // DELETE /api/Course/1 -> borra un curso.
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            bool deleted = _courseDAO.Delete(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
