using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions
{
    public interface IHubDispatcher
    {
        Task<Message> DialogCreate(Message message);
    }
}