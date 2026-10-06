using Entities;
using Microsoft.EntityFrameworkCore;

namespace DAO.entity_framework
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        // Ejercicio 1 (Deportes)
        public DbSet<Player> Players { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Team> Teams { get; set; }

        // Ejercicio 2 (Académico)
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}
