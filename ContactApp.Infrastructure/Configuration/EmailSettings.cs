
namespace ContactApp.Infrastructure.Configuration
{
    public class EmailSettings
    {
        public required string SmtpServer { get; set; }
        public int Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public required string FromEmail { get; set; }
        public required string FromName { get; set; }
        public bool UseSsl { get; set; } = true;
    }
}
