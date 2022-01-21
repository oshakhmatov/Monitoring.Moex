using Microsoft.AspNetCore.Mvc;
using Monitoring.Moex.Core.Services.EmailNotifications;

namespace Monitoring.Moex.WebApi.Controllers
{
    public class EmailNotificationController : AppController
    {
        private readonly IEmailNotificationService _emailNotificationService;

        public EmailNotificationController(IEmailNotificationService emailNotificationService)
        {
            _emailNotificationService = emailNotificationService;
        }

        [HttpPost]
        public async Task Subscribe(string email)
        {
            await _emailNotificationService.SubscribeAsync(email);
        }

        [HttpPost]
        public async Task Unsubscribe(string email)
        {
            await _emailNotificationService.UnsubscribeAsync(email);
        }
    }
}
