using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Repositories;
using Chatbot.Model.DataModel;

namespace Chatbot.Core.Services
{
    public class SettingsService : ISettingsService
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
            var shiftBegin = new Settings()
            {
                Id = new Guid("00000000-0000-0000-0001-000000000001"),
                Name = "shiftBegin",
                Description = "Дата начала смены",
                Value = "9"
            };
            var shiftEnd = new Settings()
            {
                Id = new Guid("00000000-0000-0000-0001-000000000002"),
                Name = "shiftEnd",
                Description = "Дата окончания смены",
                Value = "18"
            };
            var salam1 = new Settings()
            {
                Id = new Guid("00000000-0000-0000-0001-000000000003"),
                Name = "salam1",
                Description = "Приветствие1",
                Value = "Здравствуйте. Я чат бот АО РЭС, могу ответить на популярные вопросы."
            };
            var salam2 = new Settings()
            {
                Id = new Guid("00000000-0000-0000-0001-000000000004"),
                Name = "salam2",
                Description = "Приветствие2",
                Value = "Здравствуйте! Я робот-помощник АО РЭС. Выберите тему из списка ниже или заполните форму для связи с оператором"
            };
            var sendedMessage = new Settings()
            {
                Id = new Guid("00000000-0000-0000-0001-000000000005"),
                Name = "sendedMessage",
                Description = "Текст отправленного сообщения",
                Value = "Ваше сообщение отправлено"
            };

            var set = await GetById(new Guid("00000000-0000-0000-0001-000000000001"));
            if (set == null)
            {
                await Upsert(shiftBegin, true);
            }
            
            set = await GetById(new Guid("00000000-0000-0000-0001-000000000002"));
            if (set == null)
            {
                await Upsert(shiftEnd, true);
            }
            
            set = await GetById(new Guid("00000000-0000-0000-0001-000000000003"));
            if (set == null)
            {
                await Upsert(salam1, true);
            }
            
            set = await GetById(new Guid("00000000-0000-0000-0001-000000000004"));
            if (set == null)
            {
                await Upsert(salam2, true);
            }
            
            set = await GetById(new Guid("00000000-0000-0000-0001-000000000005"));
            if (set == null)
            {
                await Upsert(sendedMessage, true);
            }
        }
    }
}