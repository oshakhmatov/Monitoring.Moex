using Monitoring.Moex.Core.Dto.SecurityTotals;
using Monitoring.Moex.Core.Models;

namespace Monitoring.Moex.Core.DataAccess.Repos
{
    public interface ITotalsRepo : IRepo<SecurityTotal>
    {
        public Task<SecurityTotalShortDto?> GetHighestUpByClockAsync(long clock);
        public Task<SecurityTotalShortDto?> GetHighestDownByClockAsync(long clock);
        public Task<List<SecurityTotalShortDto>> ListByClockAsync(long clock);
        public Task<long?> GetMaxTradeClockAsync();
    }
}
