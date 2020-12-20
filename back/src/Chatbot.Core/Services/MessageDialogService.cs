using System.Collections.Generic;
using System.Threading.Tasks;
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

        public Task<MessageDialog> Start(MessageDialog dialog)
        {
            dialog.DialogStatus = DialogStatus.Started;
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
    }
}