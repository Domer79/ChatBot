using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts.Responses;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Core.Common;
using Chatbot.Hosting.Authentication;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chatbot.Hosting.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MessageController: ChatControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMessageDialogService _messageDialogService;
        private readonly Mapper _mapper;

        public MessageController(
            IMessageService messageService,
            IMessageDialogService messageDialogService,
            Mapper mapper)
        {
            _messageService = messageService;
            _messageDialogService = messageDialogService;
            _mapper = mapper;
        }

        [CustomSecurity(SecurityPolicy.ReadMessage)]
        [HttpGet]
        public async Task<MessageResponse[]> GetMessages(Guid messageDialogId)
        {
            var messages = await _messageService.GetDialogMessages(messageDialogId);
            return _mapper.Map<MessageResponse[]>(messages);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<MessageResponse[]> GetMessagesForUser(Guid messageDialogId)
        {
            var messages = await _messageService.GetDialogMessages(messageDialogId);
            return _mapper.Map<MessageResponse[]>(messages);
        }

        [HttpPost]
        [Authorize]
        public async Task<MessageResponse> AddOfflineMessage(Message message)
        {
            message.Time = DateTime.UtcNow;
            message.Status = MessageStatus.Received;
            await _messageDialogService.SetOffline(message.MessageDialogId);
            var msg = await _messageService.Add(message);
            return _mapper.Map<MessageResponse>(msg);
        }
    }
}