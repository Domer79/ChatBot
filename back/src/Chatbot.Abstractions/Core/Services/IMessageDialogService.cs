using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;

namespace Chatbot.Abstractions.Core.Services
{
    public interface IMessageDialogService
    {
        Task<MessageDialog> Start();
        Task<MessageDialog> Start(Guid clientId);
        Task<MessageDialog> Activate(MessageDialog dialog);
        Task<MessageDialog> Reject(MessageDialog dialog);
        Task<MessageDialog> Close(MessageDialog dialog);
        Task<MessageDialog> Activate(Guid actionsRequestMessageDialogId, Guid messageDialogId);
        Task<MessageDialog> Reject(Guid messageDialogId);
        Task<MessageDialog> Close(Guid messageDialogId);
        Task<MessageDialog> SetOffline(Guid messageDialogId);
        
        Task<MessageDialog[]> GetAll();
        Task<MessageDialog[]> GetStarted();
        Task<MessageDialog[]> GetActivities();
        Task<MessageDialog[]> GetRejected();
        Task<MessageDialog[]> GetClosed();
        Task<Page<MessageDialog>> GetPage(int number, int size);
        Task<MessageDialog> GetDialog(Guid messageDialogId);
        Task<MessageDialog[]> GetByStatusFlags(DialogStatus status);
        Task<Page<MessageDialog>> GetPageByDialogStatus(DialogStatus status, int number, int size);
    }
}