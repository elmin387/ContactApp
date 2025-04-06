using ContactApp.Application.Interfaces.Repositories;
using ContactApp.Application.Interfaces.Services;
using ContactApp.Application.Settings;
using Microsoft.Extensions.Options;

namespace ContactApp.Application.Services
{
    public class RateLimitingService: IRateLimitingService
    {
        private readonly IContactRepository _contactRepository;
        private readonly RateLimitSettings _rateLimitSettings;
        public RateLimitingService(IContactRepository contactRepository,
            IOptions<RateLimitSettings> rateLimitSettings)
        {
            _contactRepository = contactRepository;
            _rateLimitSettings = rateLimitSettings.Value;
        }
        public async Task<bool> CanSubmitAsync(string ipAddress)
        {
           
            var lastSubmission = await _contactRepository.GetLastSubmissionTimeAsync(ipAddress);

           
            var timeSinceLastSubmission = DateTime.UtcNow - lastSubmission;
            return lastSubmission == DateTime.MinValue || timeSinceLastSubmission.TotalSeconds >= _rateLimitSettings.IntervalSeconds;
        }
    }
}
