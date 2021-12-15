namespace Monitoring.Moex.Core.Models
{
    public class Security
    {
        public string? SecurityId { get; set; }
        public string? ShortName { get; set; }
        public string? Name { get; set; }
        public string? Isin { get; set; }
        public string? TypeName { get; set; }

        public List<SecurityTotal>? Totals { get; set; }

        public bool IsNotEmpty()
            => !String.IsNullOrWhiteSpace(SecurityId) && !String.IsNullOrWhiteSpace(ShortName);
    }
}
