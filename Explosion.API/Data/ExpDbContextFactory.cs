using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Explosion.API.Data
{
    public class ExpDbContextFactory : IDesignTimeDbContextFactory<ExpDbContext>
    {
        public ExpDbContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' nao encontrada para design-time DbContext.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<ExpDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new ExpDbContext(optionsBuilder.Options);
        }
    }
}
