namespace Monitoring.Moex.Core.Proccesses.EmailSender
{
    public class EmailNotificationOptions
    {
        public int Interval { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
    }
}
