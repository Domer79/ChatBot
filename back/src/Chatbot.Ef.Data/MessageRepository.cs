using System;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions.Repositories;
using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.Ef.Data
{
    public class MessageRepository: IMessageRepository
    {
        private readonly ChatbotContext _context;

        public MessageRepository(ChatbotContext context)
        {
            _context = context;
        }

        public async Task<Message> Add(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public Task<Message> GetMessage(Guid messageId)
        {
            return _context.Messages.SingleOrDefaultAsync(_ => _.Id == messageId);
        }

        public Task<Message[]> GetDialogMessages(Guid dialogId)
        {
            return _context.Messages.Where(_ => _.MessageDialogId == dialogId).ToArrayAsync();
        }
    }
}