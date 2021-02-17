using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Repositories;
using Chatbot.Common;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;

namespace Chatbot.Core.Services
{
    public class MessageDialogService: IMessageDialogService
    {
        private readonly IMessageDialogRepository _dialogRepository;

        public MessageDialogService(IMessageDialogRepository dialogRepository)
        {
            _dialogRepository = dialogRepository;
        }

        public Task<MessageDialog> Start()
        {
            var dialog = new MessageDialog {DialogStatus = DialogStatus.Started};
            return _dialogRepository.Upsert(dialog);
        }

        public Task<MessageDialog> Start(Guid clientId)
        {
            var dialog = new MessageDialog
            {
                DialogStatus = DialogStatus.Started,
                ClientId = clientId,
            };

            return _dialogRepository.Upsert(dialog);
        }

        public Task<MessageDialog> Start(Guid clientId, Guid basedId)
        {
            if (basedId == default) throw new ArgumentException(null, nameof(basedId));
            
            var dialog = new MessageDialog
            {
                DialogStatus = DialogStatus.Started,
                ClientId = clientId,
                BasedId = basedId
            };

            return _dialogRepository.Upsert(dialog);
        }

        public Task<MessageDialog> Activate(MessageDialog dialog)
        {
            dialog.DialogStatus = DialogStatus.Active;
            return _dialogRepository.Upsert(dialog);
        }

        public Task<MessageDialog> Reject(MessageDialog dialog)
        {
            dialog.DialogStatus = DialogStatus.Rejected;
            dialog.DateCompleted = DateTime.UtcNow;
            return _dialogRepository.Upsert(dialog);
        }

        public Task<MessageDialog> Close(MessageDialog dialog)
        {
            dialog.DialogStatus = DialogStatus.Closed;
            dialog.DateCompleted = DateTime.UtcNow;
            return _dialogRepository.Upsert(dialog);
        }

        public async Task<MessageDialog> Activate(Guid messageDialogId, Guid userId)
        {
            var dialog = await GetDialog(messageDialogId);
            dialog.OperatorId = userId;
            dialog.DateWork = DateTime.UtcNow;
            return await Activate(dialog);
        }

        public async Task<MessageDialog> Reject(Guid messageDialogId)
        {
            var dialog = await GetDialog(messageDialogId);
            dialog.DateCompleted = DateTime.UtcNow;
            return await Reject(dialog);
        }

        public async Task<MessageDialog> Close(Guid messageDialogId)
        {
            var dialog = await GetDialog(messageDialogId);
            return await Close(dialog);
        }

        public async Task<MessageDialog> SetOffline(Guid messageDialogId)
        {
            var dialog = await GetDialog(messageDialogId);
            dialog.Offline = true;
            return await _dialogRepository.Upsert(dialog);
        }

        public Task<MessageDialog[]> GetAll()
        {
            return _dialogRepository.GetAll();
        }

        public Task<MessageDialog[]> GetStarted()
        {
            return GetByStatusFlags(DialogStatus.Started);
        }

        public Task<MessageDialog[]> GetActivities()
        {
            return GetByStatusFlags(DialogStatus.Active);
        }

        public Task<MessageDialog[]> GetRejected()
        {
            return GetByStatusFlags(DialogStatus.Rejected);
        }

        public Task<MessageDialog[]> GetClosed()
        {
            return GetByStatusFlags(DialogStatus.Closed);
        }

        public async Task<Page<MessageDialog>> GetPage(int number, int size)
        {
            var dialogs = await _dialogRepository.GetPage(number, size);
            var totalCount = await _dialogRepository.GetTotalCount();

            return new Page<MessageDialog>()
            {
                Items = dialogs,
                TotalCount = totalCount
            };
        }

        public Task<MessageDialog> GetDialog(Guid messageDialogId)
        {
            return _dialogRepository.GetById(messageDialogId);
        }

        public async Task<MessageDialog[]> GetByStatusFlags(DialogStatus status)
        {
            var dialogs = new List<MessageDialog>();
            foreach (var value in Helper.GetFlags(status))
            {
                dialogs.AddRange(await _dialogRepository.GetByStatus(value));
            }

            return dialogs.ToArray();
        }

        public async Task<Page<MessageDialog>> GetPageByDialogStatus(DialogStatus status, int number, int size,
            bool? offline)
        {
            MessageDialog[] dialogs;
            long totalCount;
            if (offline.HasValue && offline.Value)
            {
                dialogs = await _dialogRepository.GetOffline(number, size);
                totalCount = await _dialogRepository.GetOfflineTotalCount();
            }
            else
            {
                dialogs = await _dialogRepository.GetPage(status, number, size);
                totalCount = await _dialogRepository.GetTotalCount(status);
            }

            return new Page<MessageDialog>()
            {
                Items = dialogs,
                TotalCount = totalCount
            };
        }
    }
}