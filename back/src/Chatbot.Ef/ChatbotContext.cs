using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.Ef
{
    public class ChatbotContext: DbContext
    {
        private readonly DbContextOptions _options;

        public ChatbotContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }
        
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<MessageDialog> Dialogs { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<QuestionResponse> Questions { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<OperatorLog> OperatorLogs { get; set; }
        public DbSet<Settings> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatbotContext).Assembly);
        }
    }
}
