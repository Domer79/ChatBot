﻿using System;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Repositories
{
    public interface IMessageRepository
    {
        Task<Message> Add(Message message);
        Task<Message> GetMessage(Guid messageId);
        Task<Message[]> GetDialogMessages(Guid dialogId);
    }
}