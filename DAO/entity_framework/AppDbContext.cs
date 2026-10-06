using Entities;                    // Para poder usar las clases Player, Team, Student, etc.
using Microsoft.EntityFrameworkCore; // Para DbContext y DbSet (paquete Microsoft.EntityFrameworkCore)

namespace DAO.entity_framework
{
    // AppDbContext es el "puente" entre el codigo C# y la base de datos MySQL.
    // Al heredar de DbContext, EF Core sabe:
    //  - que tablas existen (cada DbSet de abajo)
    //  - como leer, guardar, modificar y borrar datos en ellas
    // Tambien es la clase que usa "dotnet ef migrations add" para generar las migraciones.
    public class AppDbContext : DbContext
    {
        // Constructor: recibe las opciones de configuracion (que base usar y como conectarse).
        // Esas opciones se arman en Program.cs con AddDbContext + UseMySql.
        // ": base(options)" se las pasa a la clase padre DbContext, por eso las llaves van vacias.
        public AppDbContext(DbContextOptions options) : base(options) { }

        // Cada DbSet<Entidad> representa UNA TABLA de la base de datos.
        // El nombre de la propiedad (Players, Teams...) es el nombre que va a tener la tabla.
        // Para que una entidad se guarde en la base, tiene que tener su DbSet aca.

        // Ejercicio 1 (Deportes)
        public DbSet<Player> Players { get; set; }   // Tabla Players
        public DbSet<Trainer> Trainers { get; set; } // Tabla Trainers
        public DbSet<Team> Teams { get; set; }       // Tabla Teams

        // Ejercicio 2 (Académico)
        public DbSet<Student> Students { get; set; }     // Tabla Students
        public DbSet<Course> Courses { get; set; }       // Tabla Courses
        public DbSet<Activity> Activities { get; set; }  // Tabla Activities

        // Login
        public DbSet<User> Users { get; set; }           // Tabla Users

        // OnModelCreating permite configurar detalles de las tablas que EF no deduce solo.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Regla de negocio: no puede haber dos usuarios con el mismo email.
            // Se crea un INDICE UNICO: MySQL rechaza un INSERT con un email repetido.
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
