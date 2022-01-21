using Monitoring.Moex.Core.Models;

namespace Monitoring.Moex.Core.Proccesses.SecuritiesMonitoring
{
    public interface ISecuritiesParser
    {
        IEnumerable<Security> Parse(string input);
    }
}
