using Monitoring.Moex.Core.Dto.SecurityTotals;

namespace Monitoring.Moex.Core.Services.SecurityTotals
{
    public class LastTotalsViewModel
    {
        public string TradeDate { get; set; }
        public List<SecurityTotalShortDto> SecurityTotals { get; set; }
    }
}
