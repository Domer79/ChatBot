using System;
using Chatbot.Model.Enums;

namespace Chatbot.Abstractions.Contracts.Requests
{
    public class DialogPageRequest
    {
        public DialogStatus Status { get; set; }
        public int Number { get; set; }
        public int Size { get; set; }
        public bool? Offline { get; set; }
    }

    public class FilterDialogPageRequest
    {
        public DialogStatus? LinkType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public string Operator { get; set; }
        public string Client { get; set; }
        public int? DialogNumber { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}