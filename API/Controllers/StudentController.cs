using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities; // Se agrego este using
using DAO;     // Se agrego este using
using System.Collections.Generic; // Se agrego este using

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentDAO _studentDAO;

        public StudentController(StudentDAO studentDAO)
        {
            _studentDAO = studentDAO;
        }

        [HttpGet]
        public ActionResult<List<Student>> GetAll()
        {
            return Ok(_studentDAO.GetAll()); // 200 OK
        }

        [HttpGet("{id}")]
        public ActionResult<Student> GetById(long id)
        {
            var student = _studentDAO.GetById(id);
            if (student == null) return NotFound(); // 404 Not Found
            return Ok(student); // 200 OK
        }

        [HttpPost]
        public ActionResult<Student> Create([FromBody] Student student)
        {
            if (string.IsNullOrEmpty(student.Name) || string.IsNullOrEmpty(student.File))
            {
                return BadRequest("El nombre y el legajo son obligatorios."); // 400 Bad Request
            }

            var createdStudent = _studentDAO.Create(student);
            return Created($"/api/student/{createdStudent.Id}", createdStudent); // 201 Created
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Student student)
        {
            bool updated = _studentDAO.Update(id, student);
            if (!updated) return NotFound(); // 404 Not Found

            return Ok(student); // 200 OK
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            bool deleted = _studentDAO.Delete(id);
            if (!deleted) return NotFound(); // 404 Not Found

            return NoContent(); // 204 No Content
        }
    }
}
