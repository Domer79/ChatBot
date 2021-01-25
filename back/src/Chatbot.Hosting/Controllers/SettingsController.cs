using System.Threading.Tasks;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Hosting.Authentication;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Chatbot.Hosting.Controllers
{
    [CustomSecurity(SecurityPolicy.SettingsManager)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SettingsController: ChatControllerBase
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public async Task<Settings[]> GetSettings()
        {
            return await _settingsService.GetAll();
        }

        [HttpPost]
        public async Task<Settings> Upsert(Settings settings)
        {
            return await _settingsService.Upsert(settings);
        }
    }
}