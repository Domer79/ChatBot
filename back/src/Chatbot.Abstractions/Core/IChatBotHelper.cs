using System.Threading;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts.Responses;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Core
{
    public interface IChatBotHelper
    {
        Task<MessageResponse> GetResponse(Message message);
        Task<MessageResponse[]> GetQuestionMessages(Message message);
        Task<MessageResponse> GetButtonForForm(Message message);
    }
}