using Monitoring.Moex.Core.Models;

namespace Monitoring.Moex.Core.Rules.Services.LastTotalsMonitoring.Helpers
{
    public interface IParseTotalsHelper
    {
        IEnumerable<SecurityTotal> Parse(string input);
    }
}
