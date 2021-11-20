using Monitoring.Moex.Core.Models;

namespace Monitoring.Moex.Core.Dto.SecurityTotals
{
    public static class SecurityTotalExtensions
    {
        public static SecurityTotalShortDto AsShortDto(this SecurityTotal lightSecurityTotal) => new()
        {
            SecurityName = lightSecurityTotal.Security.ShortName,
            Open = lightSecurityTotal.Open!.Value,
            Close = lightSecurityTotal.Close!.Value,
            DeltaPercentage = lightSecurityTotal.OpenCloseDelta
        };
    }
}
