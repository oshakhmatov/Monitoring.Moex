using Monitoring.Moex.Core.DataAccess.Repos;
using Monitoring.Moex.Core.Models;

namespace Monitoring.Moex.Core.Services.EmailNotifications
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly IEmailSubcribersRepo _emailSubcribersRepo;

        public EmailNotificationService(IEmailSubcribersRepo emailSubcribersRepo)
        {
            _emailSubcribersRepo = emailSubcribersRepo ?? throw new ArgumentNullException(nameof(emailSubcribersRepo));
        }

        public async Task SubscribeAsync(string email)
        {
            if (email is null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            var subscriber = await _emailSubcribersRepo.GetAsync(s => s.Email == email);

            if (subscriber is not null)
            {
                return;
            }

            subscriber = new EmailSubscriber
            {
                Email = email
            };

            await _emailSubcribersRepo.AddAsync(subscriber);
        }

        public async Task UnsubscribeAsync(string email)
        {
            if (email is null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            var subscriber = await _emailSubcribersRepo.GetAsync(s => s.Email == email);

            if (subscriber is not null)
            {
                await _emailSubcribersRepo.DeleteAsync(subscriber);
            }
        }
    }
}