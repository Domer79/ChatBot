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
        DialogGroup[] GetDialogGroups(string connectionId);
        DialogGroup[] GetDialogGroups();
        bool CheckOperator(User user);
        void ConfigureDialogCreated(Func<Guid, Task> dialogCreated);
        Task RemoveDialogGroup(DialogGroup dialogGroup);
    }

    public interface IDialogCreated
    {
        Task DialogCreated(Guid messageDialogId);
    }
}