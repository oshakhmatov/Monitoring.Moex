namespace Monitoring.Moex.Core.Dto.SecurityTotals
{
    public class SecurityTotalShortDto
    {
        public string SecurityName { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double DeltaPercentage { get; set; }
    }
}
