using System;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Repositories
{
    public interface IOperatorLogRepository
    {
        Task AddLog(Guid operatorId, string action);
        Task<OperatorLog[]> GetLogs(int pageNumber, int pageSize);
        Task<long> GetTotalCount();
    }
}