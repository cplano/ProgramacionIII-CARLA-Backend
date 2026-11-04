using Microsoft.AspNetCore.Mvc;
using Entities;
using DAO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentDAO _studentDAO;

        // ASP.NET inyecta el StudentDAO registrado en Program.cs.
        public StudentController(StudentDAO studentDAO)
        {
            _studentDAO = studentDAO;
        }

        // GET /api/Student -> todos los alumnos.
        [HttpGet]
        public ActionResult<List<Student>> GetAll()
        {
            return Ok(_studentDAO.GetAll());
        }

        // GET /api/Student/1 -> un alumno por Id.
        [HttpGet("{id}")]
        public ActionResult<Student> GetById(long id)
        {
            var student = _studentDAO.GetById(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpPost]
        public ActionResult<Student> Create([FromBody] Student student)
        {
            // Validacion: nombre y legajo obligatorios ("||" = o).
            if (string.IsNullOrEmpty(student.Name) || string.IsNullOrEmpty(student.File))
            {
                return BadRequest("El nombre y el legajo son obligatorios.");
            }

            var createdStudent = _studentDAO.Create(student);
            return Created($"/api/student/{createdStudent.Id}", createdStudent);
        }

        // PUT /api/Student/1 -> modifica un alumno.
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Student student)
        {
            bool updated = _studentDAO.Update(id, student);
            if (!updated) return NotFound();

            return Ok(student);
        }

        // DELETE /api/Student/1 -> borra un alumno.
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            bool deleted = _studentDAO.Delete(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
