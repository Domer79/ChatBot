using System.IO;
using Chatbot.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Chatbot.Hosting
{
    public class DesignTimeDbContextFactory: IDesignTimeDbContextFactory<ChatbotContext>
    {
        public ChatbotContext CreateDbContext(string[] args)
        {
            var config = StartupHelper.GetConfiguration(args);
            var builder = new DbContextOptionsBuilder<ChatbotContext>();
            var connectionString = config.GetConnectionString("default");
            builder.UseSqlServer(connectionString);
            
            return new ChatbotContext(builder.Options);
        }
    }
}