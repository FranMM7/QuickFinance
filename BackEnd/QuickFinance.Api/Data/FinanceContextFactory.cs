using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using QuickFinance.Api.Data;
using System.IO;

namespace QuickFinance.Api.Data
{
    public class FinanceContextFactory : IDesignTimeDbContextFactory<FinanceContext>
    {
        public FinanceContext CreateDbContext(string[] args)
        {
            // Build configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Create DbContextOptionsBuilder with connection string
            var optionsBuilder = new DbContextOptionsBuilder<FinanceContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new FinanceContext(optionsBuilder.Options);
        }
    }
}
