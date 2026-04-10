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

        public List<User> ListEmU()
        {
            return _context.User.ToList();
        }

        public User? SearchUserId(int id)
        {
            return _context.User.Find(id);
        }

        public User Create(User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User Update(User user)
        {
            _context.User.Update(user);
            _context.SaveChanges();
            return user;
        }

        public void DeleteUser(int id)
        {
            var user = SearchUserId(id);
            if (user != null)
            {
                _context.User.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}