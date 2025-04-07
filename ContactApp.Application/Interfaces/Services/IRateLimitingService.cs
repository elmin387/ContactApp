
namespace ContactApp.Application.Interfaces.Services
{
    public interface IRateLimitingService
    {
        Task<bool> CanSubmitAsync(string ipAddress);
    }
}
