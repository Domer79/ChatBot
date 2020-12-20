using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Repositories;
using Chatbot.Model.DataModel;

namespace Chatbot.Core.Services
{
    public class MessageService: IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public Task<Message> Add(Message message)
        {
            return _messageRepository.Add(message);
        }

        public Task<Message> GetMessage(Guid messageId)
        {
            return _messageRepository.GetMessage(messageId);
        }

        public Task<Message[]> GetDialogMessages(Guid dialogId)
        {
            return _messageRepository.GetDialogMessages(dialogId);
        }
    }
}