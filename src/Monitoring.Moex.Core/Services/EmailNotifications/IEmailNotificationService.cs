namespace Monitoring.Moex.Core.Services.EmailNotifications
{
    public interface IEmailNotificationService
    {
        public Task SubscribeAsync(string email);
        public Task UnsubscribeAsync(string email);
    }
}
