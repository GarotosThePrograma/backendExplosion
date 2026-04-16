using Explosion.API.Data;
using Explosion.API.Models;

namespace Explosion.API.Repositories
{
    public class UserRep
    {
        private readonly ExpDbContext _context;

        public UserRep(ExpDbContext context)
        {
            _context = context;
        }

        public List<User> List()
        {
            return _context.Users.ToList();
        }

        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }
        public User? GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }

        public void DeleteById(int id)
        {
            var user = GetById(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
