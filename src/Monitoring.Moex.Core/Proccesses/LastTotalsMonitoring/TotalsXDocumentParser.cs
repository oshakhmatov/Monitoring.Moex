using Monitoring.Moex.Core.Extensions;
using Monitoring.Moex.Core.Models;
using System.Xml.Linq;

namespace Monitoring.Moex.Core.Proccesses.LastTotalsMonitoring
{
    public class TotalsXDocumentParser : ITotalsParser
    {
        public IEnumerable<SecurityTotal> Parse(string text)
        {
            return XDocument.Parse(text).Descendants("row").Select(row => LightSecirityTotalFrom(row));
        }

        private static SecurityTotal LightSecirityTotalFrom(XElement element)
        {
            var total = new SecurityTotal()
            {
                Open = element.Attribute("OPEN")?.AsDouble(),
                High = element.Attribute("HIGH")?.AsDouble(),
                Low = element.Attribute("LOW")?.AsDouble(),
                Close = element.Attribute("CLOSE")?.AsDouble(),
                SecurityId = element.Attribute("SECID")?.AsString()
            };

            var dateTime = element.Attribute("TRADEDATE")?.AsDatetime();

            if (dateTime is not null)
            {
                total.TradeClock = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
            }

            return total;
        }
    }
}
