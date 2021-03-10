using System;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;

namespace Chatbot.Abstractions.Repositories
{
    public interface IMessageDialogRepository
    {
        Task<MessageDialog[]> GetAll();
        Task<MessageDialog[]> GetPage(int pageNumber, int pageSize);
        Task<MessageDialog[]> GetPage(DialogStatus status, int number, int size);
        Task<long> GetTotalCount();
        Task<long> GetTotalCount(DialogStatus status);
        Task<long> GetOfflineTotalCount();
        Task<MessageDialog[]> GetByStatus(DialogStatus status);
        Task<MessageDialog> Upsert(MessageDialog dialog);
        Task<bool> Delete(MessageDialog dialog);
        Task<MessageDialog> GetById(Guid messageDialogId);
        Task<MessageDialog[]> GetOffline(int number, int size);

        Task<MessageDialog[]> GetByFilter(DialogStatus? linkType, string @operator, string client, DateTime? startDate,
            DateTime? closeDate, int? dialogNumber, int pageNumber, int pageSize);

        Task<long> GetTotalCountByFilter(DialogStatus? linkType, string @operator, string client, DateTime? startDate,
            DateTime? closeDate, int? dialogNumber, int pageNumber, int pageSize);

        Task<MessageDialog[]> GetDialogsByPeriod(DateTime startDate, DateTime endDate);
    }
}