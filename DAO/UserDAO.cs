using DAO.entity_framework;
using Entities;

namespace DAO
{
    public class UserDAO
    {
        // Contexto de EF: la conexion con la base de datos.
        private readonly AppDbContext _context;

        public UserDAO(AppDbContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User? GetById(long id)
        {
            return _context.Users.Find(id);
        }

        public User? GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public bool ExistsEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        // CREATE: guarda un usuario nuevo.
        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
    }
}
