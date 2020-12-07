using System;
using System.Security.Cryptography.X509Certificates;
using Chatbot.Model.Data;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.Data
{
    public class ChatbotDbContext: DbContext
    {
        public ChatbotDbContext(DbContextOptions options) : base(options)
        {

        }
        
        public DbSet<Message> Messages { get; set; }
    }
}
