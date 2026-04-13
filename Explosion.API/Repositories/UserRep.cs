using Explosion.API.Data;
using Explosion.API.Models;

namespace Explosion.API.Repositories
{
    public class UsersRep
    {
        private readonly ExpDbContext _context;

        public UsersRep(ExpDbContext context)
        {
            _context = context;
        }

        public List<User> ListEmU()
        {
            return _context.Users.ToList();
        }

        public User? SearchUserId(int id)
        {
            return _context.Users.Find(id);
        }
        public User? SearchUserEmail(string email)
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

        public void DeleteUser(int id)
        {
            var user = SearchUserId(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}