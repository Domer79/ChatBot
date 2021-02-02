﻿using System;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Repositories
{
    public interface ISettingsRepository
    {
        Task<Settings> GetById(Guid id);
        Task<Settings[]> GetAll();
        Task<Settings> Upsert(Settings item);
        Task<bool> Delete(Settings item);
    }
}