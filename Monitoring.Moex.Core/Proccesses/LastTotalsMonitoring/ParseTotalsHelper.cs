using Monitoring.Moex.Core.Extensions;
using Monitoring.Moex.Core.Models;
using System.Xml.Linq;

namespace Monitoring.Moex.Core.Proccesses.LastTotalsMonitoring
{
    public class ParseTotalsHelper : IParseTotalsHelper
    {
        public IEnumerable<SecurityTotal> Parse(string input)
        {
            return XDocument.Parse(input).Descendants("row").Select(row => LightSecirityTotalFrom(row));
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

            if (dateTime != null)
            {
                total.TradeClock = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
            }

            return total;
        }
    }
}
