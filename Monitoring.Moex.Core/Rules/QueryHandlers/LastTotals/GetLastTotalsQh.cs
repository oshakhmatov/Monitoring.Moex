using Monitoring.Moex.Core.DataAccess.Repos;
using Monitoring.Moex.Core.Rules.QueryHandlers.LastTotals.ViewModels;

namespace Monitoring.Moex.Core.Rules.QueryHandlers.LastTotals
{
    public class GetLastTotalsQh
    {
        private readonly ITotalsRepo _totalsRepo;

        public GetLastTotalsQh(ITotalsRepo totalsRepo)
        {
            _totalsRepo = totalsRepo;
        }

        public async Task<LastTotalsVm?> HandleAsync()
        {
            var lastTradeDate = await _totalsRepo.GetMaxTradeClockAsync();
            if (lastTradeDate == null)
                return null;

            return new LastTotalsVm()
            {
                TradeDate = DateTimeOffset.FromUnixTimeSeconds(lastTradeDate.Value).DateTime.ToShortDateString(),
                SecurityTotals = await _totalsRepo.ListByClockAsync(lastTradeDate.Value)
            };
        }
    }
}
