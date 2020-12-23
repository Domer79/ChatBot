using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Repositories;
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
                ClientId = clientId
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
            return _dialogRepository.Upsert(dialog);
        }

        public Task<MessageDialog> Close(MessageDialog dialog)
        {
            dialog.DialogStatus = DialogStatus.Closed;
            return _dialogRepository.Upsert(dialog);
        }

        public async Task<MessageDialog> Activate(Guid messageDialogId)
        {
            var dialog = await GetDialog(messageDialogId);
            return await Activate(dialog);
        }

        public async Task<MessageDialog> Reject(Guid messageDialogId)
        {
            var dialog = await GetDialog(messageDialogId);
            return await Reject(dialog);
        }

        public async Task<MessageDialog> Close(Guid messageDialogId)
        {
            var dialog = await GetDialog(messageDialogId);
            return await Close(dialog);
        }

        public Task<MessageDialog[]> GetAll()
        {
            return _dialogRepository.GetAll();
        }

        public Task<MessageDialog[]> GetStarted()
        {
            return _dialogRepository.GetByStatus(DialogStatus.Started);
        }

        public Task<MessageDialog[]> GetActivities()
        {
            return _dialogRepository.GetByStatus(DialogStatus.Active);
        }

        public Task<MessageDialog[]> GetRejected()
        {
            return _dialogRepository.GetByStatus(DialogStatus.Rejected);
        }

        public Task<MessageDialog[]> GetClosed()
        {
            return _dialogRepository.GetByStatus(DialogStatus.Closed);
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
    }
}