using Entities; // Para usar la clase Course

namespace DAO
{
    // DAO de cursos. Mismo patron CRUD que PlayerDAO (ver comentarios alli),
    // escrito con "=>" en los metodos de una sola linea (ver TeamDAO).
    public class CourseDAO
    {
        // READ: todos los cursos.
        public List<Course> GetAll() => MockDatabase.Courses;

        // READ: un curso por Id (null si no existe).
        public Course? GetById(long id) => MockDatabase.Courses.FirstOrDefault(c => c.Id == id);

        // CREATE: asigna Id (mayor + 1) y lo agrega.
        public Course Create(Course course)
        {
            course.Id = MockDatabase.Courses.Any() ? MockDatabase.Courses.Max(c => c.Id) + 1 : 1;
            MockDatabase.Courses.Add(course);
            return course;
        }

        // UPDATE: true si lo encontro y modifico, false si no existe.
        public bool Update(long id, Course updatedCourse)
        {
            var course = GetById(id);
            if (course == null) return false;

            course.Name = updatedCourse.Name;
            return true;
        }

        // DELETE: true si lo borro, false si no existe.
        public bool Delete(long id)
        {
            var course = GetById(id);
            if (course == null) return false;
            return MockDatabase.Courses.Remove(course);
        }
    }
}
