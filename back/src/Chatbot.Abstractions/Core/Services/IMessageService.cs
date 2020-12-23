using System;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Core.Services
{
    public interface IMessageService
    {
        Task<Message> Add(Message message);
        Task<Message> GetMessage(Guid messageId);
        Task<Message[]> GetDialogMessages(Guid dialogId);
        Task<Message[]> GetFirstMessages(Guid[] dialogIds);
    }
}