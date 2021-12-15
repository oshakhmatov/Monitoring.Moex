namespace Monitoring.Moex.Core.Models
{
    public class SecurityTotal
    {
        public string? SecurityId { get; set; }
        public long? TradeClock { get; set; }
        public double? Open { get; set; }
        public double? High { get; set; }
        public double? Low { get; set; }
        public double? Close { get; set; }
        public double OpenCloseDelta { get; set; }

        public Security Security { get; set; }

        public bool IsNotEmpty()
            => !String.IsNullOrWhiteSpace(SecurityId) && Close is not null && Open is not null && High is not null && Low is not null;
    }
}
