using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts.Responses;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Hosting.Authentication;
using Chatbot.Hosting.Hubs;
using Chatbot.Hosting.Misc;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Chatbot.Hosting.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MessageController: ChatControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly Mapper _mapper;

        public MessageController(
            IMessageService messageService,
            Mapper mapper)
        {
            _messageService = messageService;
            _mapper = mapper;
        }

        [CustomSecurity(SecurityPolicy.ReadMessage)]
        [HttpGet]
        public async Task<MessageResponse[]> GetMessages(Guid messageDialogId)
        {
            var messages = await _messageService.GetDialogMessages(messageDialogId);
            foreach (var message in messages)
            {
                message.Status = MessageStatus.Received;
            }
            return _mapper.Map<MessageResponse[]>(messages);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<MessageResponse[]> GetMessagesForUser(Guid messageDialogId)
        {
            var messages = await _messageService.GetDialogMessages(messageDialogId);
            return _mapper.Map<MessageResponse[]>(messages);
        }
    }
}