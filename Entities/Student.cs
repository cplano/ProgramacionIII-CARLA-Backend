namespace Entities
{
    // Student (alumno) hereda de Person: ya tiene Id, Name, Age y Dni.
    public class Student : Person
    {
        public string File { get; set; } = string.Empty; // Legajo

        // Relacion MUCHOS A MUCHOS: un alumno cursa muchos cursos
        // y un curso tiene muchos alumnos (ver Course.Students).
        // EF lo resuelve creando la tabla intermedia CourseStudent.
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
