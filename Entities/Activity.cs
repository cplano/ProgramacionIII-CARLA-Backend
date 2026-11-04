namespace Entities
{
    // Representa una actividad/tarea del curso.
    public class Activity
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }

        public TypeActivity Type { get; set; }
    }
}
