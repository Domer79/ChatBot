using System;
using System.Security.Cryptography.X509Certificates;
using Chatbot.Common.Abstracts;
using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.Data
{
    public class ChatbotContext: DbContext
    {
        private readonly IAppConfig _appConfig;

        protected ChatbotContext(IAppConfig appConfig)
        {
            _appConfig = appConfig;
        }

        public ChatbotContext(DbContextOptions options, IAppConfig appConfig) : base(options)
        {
            _appConfig = appConfig;
        }
        
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<MessageDialog> Dialogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatbotContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_appConfig.GetConnectionString());
        }
    }
}
