using Monitoring.Moex.Core.DataAccess.Repos;
using Monitoring.Moex.Core.Dto.SecurityTotals;

namespace Monitoring.Moex.Core.Services.SecurityTotals
{
    internal class SecurityTotalService : ISecurityTotalService
    {
        private readonly ITotalsRepo _totalsRepo;

        public SecurityTotalService(ITotalsRepo totalsRepo)
        {
            _totalsRepo = totalsRepo;
        }

        public async Task<LastTotalsViewModel?> GetLastTotalsAsync()
        {
            var lastTradeDate = await _totalsRepo.GetMaxTradeClockAsync();

            if (lastTradeDate == null)
            {
                return null;
            }

            return new LastTotalsViewModel()
            {
                TradeDate = DateTimeOffset.FromUnixTimeSeconds(lastTradeDate.Value).DateTime.ToShortDateString(),
                SecurityTotals = await _totalsRepo.ListByClockAsync(lastTradeDate.Value)
            };
        }

        public async Task<SecurityTotalShortDto?> GetHighestDownAsync()
        {
            var lastTradeDate = await _totalsRepo.GetMaxTradeClockAsync();

            if (lastTradeDate == null)
            {
                return null;
            }

            return await _totalsRepo.GetHighestDownByClockAsync(lastTradeDate.Value);
        }

        public async Task<SecurityTotalShortDto?> GetHighestUpAsync()
        {
            var lastTradeDate = await _totalsRepo.GetMaxTradeClockAsync();

            if (lastTradeDate == null)
            {
                return null;
            }

            return await _totalsRepo.GetHighestUpByClockAsync(lastTradeDate.Value);
        }
    }
}
