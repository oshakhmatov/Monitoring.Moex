using Monitoring.Moex.Core.DataAccess.Repos;
using Monitoring.Moex.Core.Models;

namespace Monitoring.Moex.Infrastructure.Data.Repos
{
    public class EmailSubcribersRepo : EfRepo<EmailSubscriber>, IEmailSubcribersRepo
    {
        public EmailSubcribersRepo(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
