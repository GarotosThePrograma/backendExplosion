using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Explosion.API.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<ExpDbContext>
    {
        public ExpDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ExpDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=explosiondb;Username=postgres;Password=1238");

            return new ExpDbContext(optionsBuilder.Options);
        }
    }
}