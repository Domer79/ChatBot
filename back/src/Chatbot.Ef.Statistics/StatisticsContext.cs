using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.Ef.Statistics
{
    public class StatisticsContext: DbContext
    {
        private readonly DbContextOptions _options;
        
        public DbSet<Stat> Stats { get; set; }

        public StatisticsContext(DbContextOptions<StatisticsContext> options) : base(options)
        {
            _options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StatisticsContext).Assembly);
        }
    }
}