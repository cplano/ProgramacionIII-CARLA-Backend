using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;  // Agregar este using

namespace DAO
{
    public static class MockDatabase
    {
        // Listas para el Ejercicio 1 (Deportes)
        public static List<Player> Players { get; set; } = new List<Player>
        {
            new Player { Id = 1, Name = "Lionel Messi", Age = 36, Dni = "11111111", Numero = 10, TeamId = 1 },
            new Player { Id = 2, Name = "Ángel Di María", Age = 36, Dni = "22222222", Numero = 11, TeamId = 1 }
        };

        public static List<Trainer> Trainers { get; set; } = new List<Trainer>
        {
            new Trainer { Id = 1, Name = "Lionel Scaloni", Age = 45, Dni = "33333333" }
        };

        public static List<Team> Teams { get; set; } = new List<Team>
        {
            new Team { Id = 1, Name = "Selección Argentina", Category = "Primera A" }
        };

        // Listas para el Ejercicio 2 (Académico)
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
                Date = DateTime.Now,
                Type = TypeActivity.PracticalWork
            }
        };








    }
}
