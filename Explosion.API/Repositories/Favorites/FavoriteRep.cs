using Explosion.API.Data;
using Explosion.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Explosion.API.Repositories
{
    public class FavoriteRep
    {
        private readonly ExpDbContext _context;

        public FavoriteRep(ExpDbContext context)
        {
            _context = context;
        }

        public List<Favorite> ListByUserId(int userId)
        {
            return _context.Favorites
                .Include(f => f.Product)
                .Where(f => f.UserId == userId)
                .ToList();
        }

        public bool Exists(int userId, int productId)
        {
            return _context.Favorites
                .Any(f => f.UserId == userId && f.ProductId == productId);
        }

        public Favorite Add(Favorite favorite)
        {
            _context.Favorites.Add(favorite);
            _context.SaveChanges();
            return favorite;
        }

        public Favorite? GetByUserAndProduct(int userId, int productId)
        {
            return _context.Favorites
                .FirstOrDefault(f => f.UserId == userId && f.ProductId == productId);
        }

        public void Remove(Favorite favorite)
        {
            _context.Favorites.Remove(favorite);
            _context.SaveChanges();
        }
    }
}
