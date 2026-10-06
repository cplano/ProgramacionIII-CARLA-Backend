namespace Entities
{
    // Representa la materia/curso.
    public class Course
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Relacion MUCHOS A MUCHOS con Student (tabla intermedia CourseStudent).
        public List<Student> Students { get; set; } = new List<Student>();

        // Relacion UNO A MUCHOS: un curso tiene muchas actividades.
        // EF agrega sola la columna CourseId en la tabla Activities.
        public List<Activity> Activities { get; set; } = new List<Activity>();
    }
}
