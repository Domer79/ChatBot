using System;
using System.Threading.Tasks;
using Chatbot.Model.DataModel;

namespace Chatbot.Abstractions.Core.Services
{
    public interface ISettingsService
    {
        Task<Settings> GetById(Guid id);

        Task<Settings> GetByName(string name);
        Task<Settings[]> GetAll();
        Task<Settings> Upsert(Settings item, bool justAdd = false);
        Task<bool> Delete(Settings item);
        Task SetDefaultSettings();
        Task<string> GetSalam2();
        Task<int?> GetClientTimeoutInterval();
    }
}