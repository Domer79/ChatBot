using System.Security.Claims;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions
{
    public interface IHubDispatcher
    {
        Task<Message> DialogCreate(Message message);
        Task OperatorConnect(string userIdentifier);
        Task OperatorDisconnect(string? contextUserIdentifier);
    }
}