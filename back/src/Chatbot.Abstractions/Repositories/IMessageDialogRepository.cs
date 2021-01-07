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
        Task<MessageDialog[]> GetByStatus(DialogStatus status);
        Task<MessageDialog> Upsert(MessageDialog dialog);
        Task<bool> Delete(MessageDialog dialog);
        Task<MessageDialog> GetById(Guid messageDialogId);
    }
}