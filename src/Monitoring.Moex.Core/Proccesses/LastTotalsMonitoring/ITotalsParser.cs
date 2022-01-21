using Monitoring.Moex.Core.Models;

namespace Monitoring.Moex.Core.Proccesses.LastTotalsMonitoring
{
    public interface ITotalsParser
    {
        IEnumerable<SecurityTotal> Parse(string input);
    }
}
