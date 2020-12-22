using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Core.Services
{
    public interface IMessageDialogService
    {
        Task<MessageDialog> Start();
        Task<MessageDialog> Activate(MessageDialog dialog);
        Task<MessageDialog> Reject(MessageDialog dialog);
        Task<MessageDialog> Close(MessageDialog dialog);
        
        Task<MessageDialog[]> GetAll();
        Task<MessageDialog[]> GetStarted();
        Task<MessageDialog[]> GetActivities();
        Task<MessageDialog[]> GetRejected();
        Task<MessageDialog[]> GetClosed();
    }
}