using System.IO;
using System.Threading.Tasks;
using Chatbot.Core.Services;
using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Chatbot.Ef.Data.Tests
{
    [TestFixture]
    public class MessageDialogRepositoryTest
    {
        private ChatbotContext _context;

        [OneTimeSetUp]
        public void SetUp()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var options = new DbContextOptionsBuilder()
                .UseSqlServer(config.GetConnectionString("default"))
                .Options;
            _context = new ChatbotContext(options);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CloseDialogTest()
        {
            var repo = new MessageDialogRepository(_context);
            var service = new MessageDialogService(repo);
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var messageDialog = await service.Start();
                Assert.DoesNotThrowAsync(() => service.Close(messageDialog.Id));
            }
            finally
            {
                await transaction.RollbackAsync();
            }
        }
    }
}