using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions
{
    public interface IHubDispatcher
    {
        DialogGroup GetDialogGroup(Guid messageDialogId);
        Task<DialogGroup> CreateGroup(User user, string connectionId);
        Task OperatorConnect(string userIdentifier);
        Task OperatorDisconnect(string? userIdentifier);
        DialogGroup[] GetDialogGroups(string connectionId);
        DialogGroup[] GetDialogGroups();
        bool CheckOperator(User user);
    }
}