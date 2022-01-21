namespace Monitoring.Moex.Core.Models
{
    public class EmailSubscriber
    {
        public long Id { get; set; }
        public string? Email { get; set; }
        public bool Received { get; set; }
    }
}
