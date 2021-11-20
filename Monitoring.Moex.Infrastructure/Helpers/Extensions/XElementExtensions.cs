using System.Globalization;
using System.Xml.Linq;

namespace Monitoring.Moex.Infrastructure.Helpers.Extensions
{
    public static class XElementExtensions
    {
        public static string AsString(this XElement element, string attributeName)
        {
            var attribute = element.Attribute(attributeName);
            return attribute.Value;
        }

        public static long? AsLong(this XElement element, string attributeName)
        {
            var attribute = element.Attribute(attributeName);
            return attribute.Value.Equals(String.Empty) ? null : long.Parse(attribute.Value);
        }

        public static double? AsDouble(this XElement element, string attributeName)
        {
            var attribute = element.Attribute(attributeName);
            return attribute.Value.Equals(String.Empty) ? null : double.Parse(attribute.Value, new NumberFormatInfo() { NegativeSign = "-" });
        }

        public static DateTime? AsDatetime(this XElement element, string attributeName)
        {
            var attribute = element.Attribute(attributeName);
            return attribute.Value.Equals(String.Empty) ? null : DateTime.Parse(attribute.Value);
        }
    }
}
