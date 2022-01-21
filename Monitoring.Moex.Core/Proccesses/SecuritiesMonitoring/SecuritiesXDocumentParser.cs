using Monitoring.Moex.Core.Extensions;
using Monitoring.Moex.Core.Models;
using System.Xml.Linq;

namespace Monitoring.Moex.Core.Proccesses.SecuritiesMonitoring
{
    public class SecuritiesXDocumentParser : ISecuritiesParser
    {
        public IEnumerable<Security> Parse(string text)
        {
            text = text.Replace("&quot;", @"''");

            return XDocument.Parse(text).Descendants("row").Select(row => GetSecurityFrom(row));
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
