using Explosion.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Explosion.API.Data
{
    public class ExpDbContext : DbContext
    {
        public ExpDbContext(DbContextOptions<ExpDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}