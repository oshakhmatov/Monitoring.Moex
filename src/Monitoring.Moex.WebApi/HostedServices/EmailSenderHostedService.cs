using Monitoring.Moex.Core.Proccesses.EmailSender;

namespace Monitoring.Moex.WebApi.HostedServices
{
    public class EmailSenderHostedService : BackgroundService
    {
        private readonly EmailSender _emailSender;

        public EmailSenderHostedService(EmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _emailSender.RunAsync(stoppingToken);
        }
    }
}
