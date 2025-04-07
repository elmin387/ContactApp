
namespace ContactApp.Infrastructure.Configuration
{
    public class ApiSettings
    {
        public required string BaseUrl { get; set; }
        public required string UserEndpoint { get; set; }
        public int TimeoutSeconds { get; set; } = 30;
    }
}
