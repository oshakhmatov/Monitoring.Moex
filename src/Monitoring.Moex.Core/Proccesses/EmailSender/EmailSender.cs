using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Monitoring.Moex.Core.DataAccess.Repos;
using Monitoring.Moex.Core.Services.SecurityTotals;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Monitoring.Moex.Core.Proccesses.EmailSender
{
    public class EmailSender
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IOptionsMonitor<EmailNotificationOptions> _emailNotificationOptions;

        public EmailSender(
            IServiceScopeFactory serviceScopeFactory,
            IOptionsMonitor<EmailNotificationOptions> emailNotificationOptions)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _emailNotificationOptions = emailNotificationOptions;
        }

        public async Task RunAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var options = _emailNotificationOptions.CurrentValue;

                await Task.Delay(options.Interval, stoppingToken);

                var scope = _serviceScopeFactory.CreateScope();
                var smtpClient = new SmtpClient();

                try
                {
                    var emailSubscribersRepo = scope.ServiceProvider.GetRequiredService<IEmailSubcribersRepo>();

                    var emailSubscribers = await emailSubscribersRepo.ListAsync();

                    if (!emailSubscribers.Any())
                        continue;

                    var securityTotalService = scope.ServiceProvider.GetRequiredService<ISecurityTotalService>();
                    var highestUp = await securityTotalService.GetHighestUpAsync();
                    var highestDown = await securityTotalService.GetHighestDownAsync();

                    if (highestUp is null || highestDown is null)
                        continue;

                    var body = new StringBuilder();

                    body.AppendLine("<h2>Наибольший прирост</h2>");
                    body.AppendLine($"<p>Ценная бумага: {highestUp.SecurityName}</p>");
                    body.AppendLine($"<p>Открытие: {highestUp.Open}</p>");
                    body.AppendLine($"<p>Закрытие: {highestUp.Close}</p>");
                    body.AppendLine($"<p>Изменение: {highestUp.DeltaPercentage}%</p>");

                    body.AppendLine();

                    body.AppendLine("<h2>Наибольшее падение</h2>");
                    body.AppendLine($"<p>Ценная бумага: {highestDown.SecurityName}</p>");
                    body.AppendLine($"<p>Открытие: {highestDown.Open}</p>");
                    body.AppendLine($"<p>Закрытие: {highestDown.Close}</p>");
                    body.AppendLine($"<p>Изменение: {highestDown.DeltaPercentage}%</p>");

                    smtpClient.Host = options.Host;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential()
                    {
                        UserName = options.Address,
                        Password = options.Password
                    };

                    Parallel.ForEach(emailSubscribers, (s) =>
                    {
                        if (!s.Received)
                        {
                            var to = new MailAddress(s.Email);
                            var from = new MailAddress(options.Address);

                            var message = new MailMessage(from, to)
                            {
                                Subject = "Moex monitoring notification",
                                IsBodyHtml = true,
                                Body = body.ToString()
                            };

                            smtpClient.Send(message);

                            s.Received = true;
                        }
                    });

                    await emailSubscribersRepo.UpdateRangeAsync(emailSubscribers);
                }
                catch (Exception ex)
                {
                    // to do log
                }
                finally
                {
                    scope?.Dispose();
                    smtpClient?.Dispose();
                }
            }
        }
    }
}
