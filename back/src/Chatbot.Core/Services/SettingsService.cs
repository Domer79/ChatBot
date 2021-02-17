using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Repositories;
using Chatbot.Core.Common;
using Chatbot.Model.DataModel;

namespace Chatbot.Core.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _settingsRepository;
        private string _salam2;
        private double? _clientTimeoutInterval;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public Task<Settings> GetById(Guid id)
        {
            return _settingsRepository.GetById(id);
        }

        public Task<Settings> GetByName(string name)
        {
            return _settingsRepository.GetByName(name);
        }

        public Task<Settings[]> GetAll()
        {
            return _settingsRepository.GetAll();
        }

        public Task<Settings> Upsert(Settings item, bool justAdd = false)
        {
            return _settingsRepository.Upsert(item, justAdd);
        }

        public Task<bool> Delete(Settings item)
        {
            return _settingsRepository.Delete(item);
        }

        public async Task SetDefaultSettings()
        {
            foreach (var setting in DefaultSettingsSource.GetDefaultSettings())
            {
                var set = await GetByName(setting.Name);
                if (set == null)
                {
                    await Upsert(setting, true);
                }
            }
        }

        public async Task<string> GetSalam2()
        {
            return _salam2 ??= (await GetByName("salam2")).Value;
        }

        public async Task<double?> GetClientTimeoutInterval()
        {
            var value = (await GetByName("clientTimeoutInterval")).Value;
            return _clientTimeoutInterval ??= double.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}