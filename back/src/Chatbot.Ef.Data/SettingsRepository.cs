using System;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions.Repositories;
using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.Ef.Data
{
    public class SettingsRepository: ISettingsRepository
    {
        private readonly ChatbotContext _context;

        public SettingsRepository(ChatbotContext context)
        {
            _context = context;
        }

        public Task<Settings> GetById(Guid id)
        {
            return _context.Settings.FindAsync(id).AsTask();
        }

        public Task<Settings[]> GetAll()
        {
            return _context.Settings.ToArrayAsync();
        }

        public async Task<Settings> Upsert(Settings item, bool justAdd = false)
        {
            if (item.Id == Guid.Empty)
            {
                item.Id = Guid.NewGuid();
                _context.Add(item);
            }
            else if (justAdd)
            {
                _context.Add(item);
            }
            else
            {
                _context.Update(item);
            }
            
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> Delete(Settings item)
        {
            _context.Remove(item);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}