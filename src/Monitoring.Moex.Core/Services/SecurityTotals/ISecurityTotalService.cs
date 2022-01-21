using Monitoring.Moex.Core.Dto.SecurityTotals;

namespace Monitoring.Moex.Core.Services.SecurityTotals
{
    public interface ISecurityTotalService
    {
        public Task<LastTotalsViewModel?> GetLastTotalsAsync();

        public Task<SecurityTotalShortDto?> GetHighestUpAsync();

        public Task<SecurityTotalShortDto> GetHighestDownAsync();
    }
}
