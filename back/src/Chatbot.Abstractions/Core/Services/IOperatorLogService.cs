using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Core.Services
{
    public interface IOperatorLogService
    {
        Task Log(Guid operatorId, string action);
        Task<Page<OperatorLog>> GetLogs(int pageNumber, int pageSize);
        Task Log(string login, string action);
    }
}