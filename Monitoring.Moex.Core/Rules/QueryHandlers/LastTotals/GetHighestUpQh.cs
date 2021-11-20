using Monitoring.Moex.Core.DataAccess.Repos;
using Monitoring.Moex.Core.Dto.SecurityTotals;

namespace Monitoring.Moex.Core.Rules.QueryHandlers.LastTotals
{
    public class GetHighestUpQh
    {
        private readonly ITotalsRepo _totalsRepo;

        public GetHighestUpQh(ITotalsRepo totalsRepo)
        {
            _totalsRepo = totalsRepo;
        }

        public async Task<SecurityTotalShortDto?> HandleAsync()
        {
            var lastTradeDate = await _totalsRepo.GetMaxTradeClockAsync();
            if (lastTradeDate == null)
                return null;

            return await _totalsRepo.GetHighestUpByClockAsync(lastTradeDate.Value);
        }
    }
}
