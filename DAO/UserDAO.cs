using DAO.entity_framework; // Para usar AppDbContext
using Entities;            // Para usar la clase User

namespace DAO
{
    // DAO de usuarios. A diferencia de los otros DAO (que usan MockDatabase),
    // este trabaja con la BASE DE DATOS REAL (MySQL) a traves de Entity Framework.
    public class UserDAO
    {
        // Contexto de EF: la conexion con la base de datos.
        private readonly AppDbContext _context;

        // INYECCION DE DEPENDENCIAS: ASP.NET pasa el AppDbContext registrado en Program.cs.
        public UserDAO(AppDbContext context)
        {
            _context = context;
        }

        // READ: todos los usuarios.
        // _context.Users es el DbSet (la tabla Users). ToList() ejecuta: SELECT * FROM Users
        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        // READ: un usuario por Id. Find busca por clave primaria (null si no existe).
        public User? GetById(long id)
        {
            return _context.Users.Find(id);
        }

        // READ: busca por email (lo usa el login).
        // EF traduce esta consulta LINQ a SQL: SELECT ... FROM Users WHERE Email = @email LIMIT 1
        public User? GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        // Devuelve true si ya existe un usuario con ese email (regla de negocio: email unico).
        // Any() se traduce a SQL como: SELECT EXISTS(...)
        public bool ExistsEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        // CREATE: guarda un usuario nuevo.
        public User Create(User user)
        {
            _context.Users.Add(user); // Lo marca para insertar (todavia no va a la base)
            _context.SaveChanges();   // Ejecuta el INSERT en MySQL. MySQL genera el Id (AUTO_INCREMENT)
            return user;              // EF ya le cargo el Id generado
        }
    }
}
