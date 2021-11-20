using Monitoring.Moex.Core.Dto.SecurityTotals;

namespace Monitoring.Moex.Core.Rules.QueryHandlers.LastTotals.ViewModels
{
    public class LastTotalsVm
    {
        public string TradeDate { get; set; }
        public List<SecurityTotalShortDto> SecurityTotals { get; set; }
    }
}
