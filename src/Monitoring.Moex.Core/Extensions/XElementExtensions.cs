using System.Globalization;
using System.Xml.Linq;

namespace Monitoring.Moex.Core.Extensions
{
    public static class XElementExtensions
    {
        public static string? AsString(this XAttribute attribute)
        {
            return attribute?.Value;
        }

        public static long? AsLong(this XAttribute attribute)
        {
            return String.IsNullOrWhiteSpace(attribute.Value) ? null : long.Parse(attribute.Value);
        }

        public static double? AsDouble(this XAttribute attribute)
        {
            return String.IsNullOrWhiteSpace(attribute.Value) ? null : double.Parse(attribute.Value, new NumberFormatInfo() { NegativeSign = "-" });
        }

        public static DateTime? AsDatetime(this XAttribute attribute)
        {
            return String.IsNullOrWhiteSpace(attribute.Value) ? null : DateTime.Parse(attribute.Value);
        }
    }
}
