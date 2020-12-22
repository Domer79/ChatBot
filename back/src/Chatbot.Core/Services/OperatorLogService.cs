using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Repositories;
using Chatbot.Model.DataModel;

namespace Chatbot.Core.Services
{
    public class OperatorLogService: IOperatorLogService
    {
        private readonly IOperatorLogRepository _logRepository;
        private readonly IUserService _userService;

        public OperatorLogService(IOperatorLogRepository logRepository, IUserService userService)
        {
            _logRepository = logRepository;
            _userService = userService;
        }

        public Task Log(Guid operatorId, string action)
        {
            return _logRepository.AddLog(operatorId, action);
        }

        public async Task<Page<OperatorLog>> GetLogs(int pageNumber, int pageSize)
        {
            var logs = await _logRepository.GetLogs(pageNumber, pageSize);
            var totalCount = await _logRepository.GetTotalCount();

            return new Page<OperatorLog>()
            {
                Items = logs,
                TotalCount = totalCount
            };
        }

        public async Task Log(string login, string action)
        {
            var user = await _userService.GetByLoginOrEmail(login);
            await Log(user.Id, action);
        }
    }
}