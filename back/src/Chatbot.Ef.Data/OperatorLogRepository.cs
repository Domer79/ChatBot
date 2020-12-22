using System;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions.Repositories;
using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.Ef.Data
{
    public class OperatorLogRepository: IOperatorLogRepository
    {
        private readonly ChatbotContext _context;

        public OperatorLogRepository(ChatbotContext context)
        {
            _context = context;
        }

        public Task AddLog(Guid operatorId, string action)
        {
            _context.OperatorLogs.Add(new OperatorLog() {OperatorId = operatorId, Action = action});
            return _context.SaveChangesAsync();
        }

        public Task<OperatorLog[]> GetLogs(int pageNumber, int pageSize)
        {
            return _context.OperatorLogs.Skip(pageNumber * pageSize - pageSize).Take(pageSize).ToArrayAsync();
        }

        public Task<long> GetTotalCount()
        {
            return _context.OperatorLogs.LongCountAsync();
        }
    }
}