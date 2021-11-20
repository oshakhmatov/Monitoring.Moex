using Monitoring.Moex.Core.Models;

namespace Monitoring.Moex.Core.Rules.Services.SecuritiesMonitoring.Helpers
{
    public interface IParseSecuritiesHelper
    {
        IEnumerable<Security> Parse(string input);
    }
}
