using Entities; // Para usar las clases de la capa Entities

namespace DAO
{
    // MockDatabase es una base de datos FALSA ("mock" = simulacro) que vive en memoria.
    // Son listas comunes de C#: los datos se pierden cada vez que se apaga la API.
    // Se usa para probar los DAO y la API antes de tener una base real (MySQL).
    //
    // "static" = hay una sola copia compartida por toda la aplicacion,
    // y se usa sin crear objetos: MockDatabase.Players
    public static class MockDatabase
    {
        // ---------------- Ejercicio 1 (Deportes) ----------------

        // Lista de jugadores con dos datos de ejemplo cargados desde el inicio.
        public static List<Player> Players { get; set; } = new List<Player>
        {
            new Player { Id = 1, Name = "Lionel Messi", Age = 36, Dni = "11111111", Numero = 10, TeamId = 1 },
            new Player { Id = 2, Name = "Ángel Di María", Age = 36, Dni = "22222222", Numero = 11, TeamId = 1 }
        };

        public static List<Trainer> Trainers { get; set; } = new List<Trainer>
        {
            new Trainer { Id = 1, Name = "Lionel Scaloni", Age = 45, Dni = "33333333" }
        };

        // El equipo con Id = 1 es al que apuntan los jugadores con TeamId = 1.
        public static List<Team> Teams { get; set; } = new List<Team>
        {
            new Team { Id = 1, Name = "Selección Argentina", Category = "Primera A" }
        };

        // ---------------- Ejercicio 2 (Académico) ----------------

        public static List<Student> Students { get; set; } = new List<Student>
        {
            new Student { Id = 1, Name = "Gastón Rosales", Age = 25, Dni = "44444444", File = "ALU-101" }
        };

        public static List<Course> Courses { get; set; } = new List<Course>
        {
            new Course { Id = 1, Name = "Programación Backend C#" }
        };

        public static List<Activity> Activities { get; set; } = new List<Activity>
        {
            new Activity {
                Id = 1,
                Title = "TP Arquitectura por Capas",
                Description = "Implementar DAO y API",
                Date = DateTime.Now,               // Fecha y hora actual
                Type = TypeActivity.PracticalWork  // Valor del enum
            }
        };
    }
}
