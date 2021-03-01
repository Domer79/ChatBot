using System;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Repositories
{
    public interface IMessageRepository
    {
        Task<Message> Add(Message message);
        Task<bool> Update(Message message);
        Task<Message> GetMessage(Guid messageId);
        Task<Message[]> GetDialogMessages(Guid dialogId);
        Task<Message[]> GetFirstMessages(Guid[] dialogIds);
        Task<Message[]> GetLastMessages(Guid[] dialogIds);
        Task<Message[]> GetAllMessagesInLinkedDialogsByNumber(int dialogNumber);
    }
}