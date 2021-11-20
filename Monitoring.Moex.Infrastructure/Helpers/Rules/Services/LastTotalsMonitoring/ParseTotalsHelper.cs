using Monitoring.Moex.Core.Models;
using Monitoring.Moex.Core.Rules.Services.LastTotalsMonitoring.Helpers;
using Monitoring.Moex.Infrastructure.Helpers.Extensions;
using System.Xml.Linq;

namespace Monitoring.Moex.Infrastructure.Helpers.Rules.Services.LastTotalsMonitoring
{
    public class ParseTotalsHelper : IParseTotalsHelper
    {
        public IEnumerable<SecurityTotal> Parse(string input) =>
            XDocument.Parse(input).Descendants("row").Select(row => LightSecirityTotalFrom(row));

        private static SecurityTotal LightSecirityTotalFrom(XElement element) => new()
        {
            TradeClock = element.AsDatetime("TRADEDATE") == null ? null : ((DateTimeOffset) element.AsDatetime("TRADEDATE")!).ToUnixTimeSeconds(),
            Open = element.AsDouble("OPEN"),
            High = element.AsDouble("HIGH"),
            Low = element.AsDouble("LOW"),
            Close = element.AsDouble("CLOSE"),
            SecurityId = element.AsString("SECID")
        };
    }
}
