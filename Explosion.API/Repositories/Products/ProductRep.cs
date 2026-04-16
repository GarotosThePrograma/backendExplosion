using Explosion.API.Data;
using Explosion.API.Models;

namespace Explosion.API.Repositories
{
    public class ProductRep
    {
        private readonly ExpDbContext _context;

        public ProductRep(ExpDbContext context)
        {
            _context = context;
        }

        public List<Product> List()
        {
            return _context.Products.ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products.Find(id);
        }

        public Product? GetByName(string name)
        {
            return _context.Products.FirstOrDefault(p => p.Name == name);
        }

        public Product Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
            return product;
        }

        public void Remove(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
