using Monitoring.Moex.Core.Models;
using Monitoring.Moex.Core.Rules.Services.SecuritiesMonitoring.Helpers;
using Monitoring.Moex.Infrastructure.Helpers.Extensions;
using System.Text;
using System.Xml.Linq;

namespace Monitoring.Moex.Infrastructure.Helpers.Rules.Services.SecuritiesMonitoring
{
    public class ParseSecuritiesHelper : IParseSecuritiesHelper
    {
        public IEnumerable<Security> Parse(string input)
        {
            var cleanInput = Clean(input);

            return XDocument.Parse(cleanInput).Descendants("row").Select(row => GetSecurityFrom(row));
        }

        private static string Clean(string input)
        {
            var sb = new StringBuilder(input);
            sb.Replace("&quot;", @"''");
            return sb.ToString();
        }

        private static Security GetSecurityFrom(XElement element) => new()
        {
            SecurityId = element.AsString("SECID"),
            Name = element.AsString("NAME"),
            ShortName = element.AsString("SHORTNAME"),
            Isin = element.AsString("ISIN"),
            TypeName = element.AsString("TYPENAME")
        };
    }
}
