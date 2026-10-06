namespace Entities
{
    // Un ENUM es una lista cerrada de opciones posibles.
    // Sirve para que Type solo pueda tomar uno de estos valores (y no cualquier texto).
    // Internamente cada opcion es un numero que empieza en 0.
    public enum TypeActivity
    {
        Exam,          // 0 - Examen
        Homework,      // 1 - Tarea
        PracticalWork, // 2 - Trabajo practico
        Quiz           // 3 - Cuestionario
    }
}
