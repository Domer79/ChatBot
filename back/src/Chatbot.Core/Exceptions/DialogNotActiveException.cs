using System;
using Chatbot.Model.Enums;

namespace Chatbot.Core.Exceptions
{
    public class DialogNotActiveException: Exception
    {
        public DialogNotActiveException(Guid dialogId, int dialogNumber, DialogStatus dialogStatus)
            : base($"Диалог №{dialogNumber} не активен (DialogId: {dialogId}).")
        {
            DialogStatus = dialogStatus;
            DialogId = dialogId;
            DialogNumber = dialogNumber;
        }

        public int DialogNumber { get; }

        public Guid DialogId { get; }

        public DialogStatus DialogStatus { get; }
    }
}