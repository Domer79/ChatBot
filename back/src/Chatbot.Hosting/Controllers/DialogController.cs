using System;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Contracts.Responses;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Hosting.Authentication;
using Chatbot.Hosting.Misc;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Chatbot.Hosting.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [CustomSecurity(SecurityPolicy.DialogPage)]
    public class DialogController: ChatControllerBase
    {
        private readonly IMessageDialogService _messageDialogService;
        private readonly IMessageService _messageService;
        private readonly Mapper _mapper;

        public DialogController(
            IMessageDialogService messageDialogService, 
            IMessageService messageService,
            Mapper mapper)
        {
            _messageDialogService = messageDialogService;
            _messageService = messageService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<Page<MessageDialogResponse>> GetDialogs(PageRequest request)
        {
            var page = await _messageDialogService.GetPage(request.Number, request.Size);

            return new Page<MessageDialogResponse>()
            {
                Items = _mapper.Map<MessageDialogResponse[]>(page.Items),
                TotalCount = page.TotalCount
            };
        }

        [HttpGet]
        public async Task<MessageDialogResponse[]> GetStartedDialogs()
        {
            var dialogs = await _messageDialogService.GetStarted();
            Message[] messages = Array.Empty<Message>();
            if (dialogs.Length > 0)
                messages = await _messageService.GetFirstMessages(dialogs.Select(_ => _.Id).ToArray());
            return dialogs.Select((_, index) =>
            {
                var dialogResponse = _mapper.Map<MessageDialogResponse>(_);
                dialogResponse.FirstMessage = messages.FirstOrDefault(m => m.MessageDialogId == _.Id)?.Content;
                return dialogResponse;
            }).ToArray();
        }
    }
}