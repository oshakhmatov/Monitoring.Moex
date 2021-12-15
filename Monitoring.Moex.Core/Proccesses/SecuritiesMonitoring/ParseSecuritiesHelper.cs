using Monitoring.Moex.Core.Extensions;
using Monitoring.Moex.Core.Models;
using System.Xml.Linq;

namespace Monitoring.Moex.Core.Proccesses.SecuritiesMonitoring
{
    public class ParseSecuritiesHelper : IParseSecuritiesHelper
    {
        public IEnumerable<Security> Parse(string input)
        {
            input = input.Replace("&quot;", @"''");

            return XDocument.Parse(input).Descendants("row").Select(row => GetSecurityFrom(row));
        }

        private static Security GetSecurityFrom(XElement element)
        {
            return new Security()
            {
                SecurityId = element.Attribute("SECID")?.AsString(),
                Name = element.Attribute("NAME")?.AsString(),
                ShortName = element.Attribute("SHORTNAME")?.AsString(),
                Isin = element.Attribute("ISIN")?.AsString(),
                TypeName = element.Attribute("TYPENAME")?.AsString()
            };
        }
    }
}
