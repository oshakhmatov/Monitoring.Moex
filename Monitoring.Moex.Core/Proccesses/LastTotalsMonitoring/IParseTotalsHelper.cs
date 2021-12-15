using Monitoring.Moex.Core.Models;

namespace Monitoring.Moex.Core.Proccesses.LastTotalsMonitoring
{
    public interface IParseTotalsHelper
    {
        IEnumerable<SecurityTotal> Parse(string input);
    }
}
