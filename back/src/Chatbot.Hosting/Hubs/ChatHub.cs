using System;
using System.Threading.Tasks;
using Chatbot.Abstractions;
using Chatbot.Abstractions.Enums;
using Chatbot.Model.DataModel;
using Microsoft.AspNetCore.SignalR;

namespace Chatbot.Hosting.Hubs
{
    public class ChatHub: Hub
    {
        
    }

    public class DialogHub : Hub
    {
        public async Task TakeToWork()
        {
            var user = Context.User;
        }
    }

    public class HubDispatcher : IHubDispatcher
    {
        private readonly DialogHub _dialogHub;

        public HubDispatcher(DialogHub dialogHub)
        {
            _dialogHub = dialogHub;
        }

        public async Task<Message> DialogCreate(Message message)
        {
            var messageDialog = new MessageDialog()
            {
                DialogStatus = (int) DialogStatus.Started,
            };
            
            // TODO: Тут должно быть реализовано сохранение
            
            await _dialogHub.Clients.Others.SendAsync("Send", messageDialog);
            return message;
        }
    }
}