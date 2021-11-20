using Monitoring.Moex.Core.DataAccess.Repos;
using Monitoring.Moex.Core.Dto.SecurityTotals;

namespace Monitoring.Moex.Core.Rules.QueryHandlers.LastTotals
{
    public class GetHighestDownQh
    {
        private readonly ITotalsRepo _totalsRepo;

        public GetHighestDownQh(ITotalsRepo totalsRepo)
        {
            _totalsRepo = totalsRepo;
        }

        public async Task<SecurityTotalShortDto?> HandleAsync()
        {
            var lastTradeDate = await _totalsRepo.GetMaxTradeClockAsync();
            if (lastTradeDate == null)
                return null;

            return await _totalsRepo.GetHighestDownByClockAsync(lastTradeDate.Value);
        }
    }
}
