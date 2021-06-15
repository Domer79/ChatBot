using Chatbot.Ef.Statistics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chatbot.Helper
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
    
    public class StatisticsContextFactory: IDesignTimeDbContextFactory<StatisticsContext>
    {
        IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        public StatisticsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StatisticsContext>()
                .UseSqlServer(configuration.GetConnectionString("Stat"));

            return new StatisticsContext(optionsBuilder.Options);
        }
    }
}