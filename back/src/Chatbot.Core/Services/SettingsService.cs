using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Repositories;
using Chatbot.Model.DataModel;

namespace Chatbot.Core.Services
{
    public class SettingsService: ISettingsService
    {
        private readonly ISettingsRepository _settingsRepository;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public Task<Settings> GetById(Guid id)
        {
            return _settingsRepository.GetById(id);
        }

        public Task<Settings[]> GetAll()
        {
            return _settingsRepository.GetAll();
        }

        public Task<Settings> Upsert(Settings item)
        {
            return _settingsRepository.Upsert(item);
        }

        public Task<bool> Delete(Settings item)
        {
            return _settingsRepository.Delete(item);
        }
    }
}