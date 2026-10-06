using Microsoft.AspNetCore.Mvc; // Para ControllerBase, [ApiController], [HttpGet], Ok(), NotFound(), etc.
using Entities;                 // Para usar la clase Student
using DAO;                      // Para usar StudentDAO

namespace API.Controllers
{
    // Controller de alumnos. Misma estructura que PlayerController (ver comentarios alli).
    // URL base: /api/Student
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
            return Ok(_studentDAO.GetAll()); // 200 OK
        }

        // GET /api/Student/1 -> un alumno por Id.
        [HttpGet("{id}")]
        public ActionResult<Student> GetById(long id)
        {
            var student = _studentDAO.GetById(id);
            if (student == null) return NotFound(); // 404 Not Found
            return Ok(student); // 200 OK
        }

        // POST /api/Student -> crea un alumno (JSON en el cuerpo del pedido).
        [HttpPost]
        public ActionResult<Student> Create([FromBody] Student student)
        {
            // Validacion: nombre y legajo obligatorios ("||" = o).
            if (string.IsNullOrEmpty(student.Name) || string.IsNullOrEmpty(student.File))
            {
                return BadRequest("El nombre y el legajo son obligatorios."); // 400 Bad Request
            }

            var createdStudent = _studentDAO.Create(student);
            return Created($"/api/student/{createdStudent.Id}", createdStudent); // 201 Created
        }

        // PUT /api/Student/1 -> modifica un alumno.
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Student student)
        {
            bool updated = _studentDAO.Update(id, student);
            if (!updated) return NotFound(); // 404 Not Found

            return Ok(student); // 200 OK
        }

        // DELETE /api/Student/1 -> borra un alumno.
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            bool deleted = _studentDAO.Delete(id);
            if (!deleted) return NotFound(); // 404 Not Found

            return NoContent(); // 204 No Content
        }
    }
}
