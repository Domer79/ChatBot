using System;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions.Repositories;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.Ef.Data
{
    public class MessageDialogRepository: IMessageDialogRepository
    {
        private readonly ChatbotContext _context;

        public MessageDialogRepository(ChatbotContext context)
        {
            _context = context;
        }

        public Task<MessageDialog[]> GetAll()
        {
            return _context.Dialogs.ToArrayAsync();
        }

        public Task<MessageDialog[]> GetByStatus(DialogStatus status)
        {
            return _context.Dialogs.Where(_ => _.DialogStatus == status).ToArrayAsync();
        }

        public async Task<MessageDialog> Upsert(MessageDialog dialog)
        {
            if (dialog.Id == Guid.Empty)
            {
                dialog.Id = Guid.NewGuid();
                _context.Dialogs.Add(dialog);
            }
            else
            {
                _context.Update(dialog);
            }
            
            await _context.SaveChangesAsync();
            return dialog;
        }

        public async Task<bool> Delete(MessageDialog dialog)
        {
            _context.Dialogs.Remove(dialog);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}