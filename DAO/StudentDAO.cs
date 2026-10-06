using Entities; // Para usar la clase Student

namespace DAO
{
    // DAO de alumnos. Mismo patron CRUD que PlayerDAO (ver comentarios alli),
    // escrito con "=>" en los metodos de una sola linea (ver TeamDAO).
    public class StudentDAO
    {
        // READ: todos los alumnos.
        public List<Student> GetAll() => MockDatabase.Students;

        // READ: un alumno por Id (null si no existe).
        public Student? GetById(long id) => MockDatabase.Students.FirstOrDefault(s => s.Id == id);

        // CREATE: asigna Id (mayor + 1) y lo agrega.
        public Student Create(Student student)
        {
            student.Id = MockDatabase.Students.Any() ? MockDatabase.Students.Max(s => s.Id) + 1 : 1;
            MockDatabase.Students.Add(student);
            return student;
        }

        // UPDATE: true si lo encontro y modifico, false si no existe.
        public bool Update(long id, Student updatedStudent)
        {
            var student = GetById(id);
            if (student == null) return false;

            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.Dni = updatedStudent.Dni;
            student.File = updatedStudent.File; // Legajo
            return true;
        }

        // DELETE: true si lo borro, false si no existe.
        public bool Delete(long id)
        {
            var student = GetById(id);
            if (student == null) return false;
            return MockDatabase.Students.Remove(student);
        }
    }
}
