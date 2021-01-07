using System;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Contracts.Requests;
using Chatbot.Abstractions.Contracts.Responses;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Hosting.Authentication;
using Chatbot.Hosting.Misc;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Chatbot.Hosting.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [CustomSecurity(SecurityPolicy.DialogPage)]
    public class DialogController: ChatControllerBase
    {
        private readonly IMessageDialogService _messageDialogService;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly Mapper _mapper;

        public DialogController(
            IMessageDialogService messageDialogService, 
            IMessageService messageService,
            IUserService userService,
            Mapper mapper)
        {
            _messageDialogService = messageDialogService;
            _messageService = messageService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<Page<MessageDialogResponse>> GetDialogs([FromQuery]PageRequest request)
        {
            var page = await _messageDialogService.GetPage(request.Number, request.Size);

            return new Page<MessageDialogResponse>()
            {
                Items = _mapper.Map<MessageDialogResponse[]>(page.Items),
                TotalCount = page.TotalCount
            };
        }

        [HttpGet]
        public async Task<Page<MessageDialogResponse>> GetDialogsByStatus([FromQuery] DialogPageRequest request)
        {
            Page<MessageDialog> page = await _messageDialogService.GetPageByDialogStatus(request.Status, request.Number, request.Size);
            var ids = page.Items.Select(_ => _.ClientId).OfType<Guid>().ToArray();
            var users = (await _userService.GetByIds(ids)).ToDictionary(_ => _.Id, _ => _);

            foreach (var dialog in page.Items)
            {
                if (dialog.ClientId == null) continue;

                var clientId = dialog.ClientId.Value;
                dialog.Client = users[clientId];
            }
            
            return new Page<MessageDialogResponse>()
            {
                Items = _mapper.Map<MessageDialogResponse[]>(page.Items),
                TotalCount = page.TotalCount
            };
        }

        [HttpGet]
        public async Task<MessageDialogResponse[]> GetStartedOrActiveDialogs()
        {
            var dialogs = await _messageDialogService.GetByStatusFlags(DialogStatus.Started | DialogStatus.Active);
            Message[] messages = Array.Empty<Message>();
            if (dialogs.Length > 0)
                messages = await _messageService.GetFirstMessages(dialogs.Select(_ => _.Id).ToArray());
            return dialogs.Select(_ =>
            {
                var dialogResponse = _mapper.Map<MessageDialogResponse>(_);
                dialogResponse.FirstMessage = messages.FirstOrDefault(m => m.MessageDialogId == _.Id)?.Content;
                return dialogResponse;
            }).ToArray();
        }

        [HttpPost]
        public async Task Activate([FromBody] DialogActionsRequest actionsRequest)
        {
            if (UserId == null)
                throw new InvalidOperationException("User not identified");
            
            await _messageDialogService.Activate(actionsRequest.MessageDialogId, UserId.Value);
        }

        [HttpPost]
        public async Task Reject([FromBody] DialogActionsRequest actionsRequest)
        {
            await _messageDialogService.Reject(actionsRequest.MessageDialogId);
        }
    }
}