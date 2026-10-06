namespace Entities
{
    // Representa una actividad/tarea del curso.
    public class Activity
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; } // Fecha y hora de la actividad

        // Tipo de actividad, usando el enum TypeActivity.
        // En la base se guarda como numero (Exam=0, Homework=1, ...).
        public TypeActivity Type { get; set; }
    }
}
