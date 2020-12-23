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
        Task<long> GetTotalCount();
        Task<MessageDialog[]> GetByStatus(DialogStatus status);
        Task<MessageDialog> Upsert(MessageDialog dialog);
        Task<bool> Delete(MessageDialog dialog);
        Task<MessageDialog> GetById(Guid messageDialogId);
    }
}