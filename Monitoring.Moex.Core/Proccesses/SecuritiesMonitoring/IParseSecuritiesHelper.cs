using Monitoring.Moex.Core.Models;

namespace Monitoring.Moex.Core.Proccesses.SecuritiesMonitoring
{
    public interface IParseSecuritiesHelper
    {
        IEnumerable<Security> Parse(string input);
    }
}
