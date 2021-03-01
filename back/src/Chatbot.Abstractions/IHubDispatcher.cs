using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Contracts.Chat;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions
{
    public interface IHubDispatcher
    {
        Task<IDialogGroup> GetActiveDialogGroup(Guid messageDialogId);
        
        Task<IDialogGroup> GetOrCreateDialogGroup(User user, Guid messageDialogId);

        IDialogGroup[] GetDeprecated();

        Task CloseClientDialog(Guid userId, Guid? messageDialogId = null);
    }
}