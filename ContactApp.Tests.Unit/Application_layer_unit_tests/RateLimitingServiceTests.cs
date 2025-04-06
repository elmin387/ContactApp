using ContactApp.Application.Interfaces.Repositories;
using ContactApp.Application.Services;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using ContactApp.Application.Settings;


namespace ContactApp.Tests.Unit.Application_layer_tests
{
    public class RateLimitingServiceTests
    {
        private readonly Mock<IContactRepository> _mockContactRepository;
        private readonly RateLimitSettings _rateLimitSettings;
        private readonly RateLimitingService _rateLimitingService;

        public RateLimitingServiceTests()
        {
            _mockContactRepository = new Mock<IContactRepository>();
            _rateLimitSettings = new RateLimitSettings { IntervalSeconds = 60 };

            var mockOptions = new Mock<IOptions<RateLimitSettings>>();
            mockOptions.Setup(x => x.Value).Returns(_rateLimitSettings);

            _rateLimitingService = new RateLimitingService(
                _mockContactRepository.Object,
                mockOptions.Object
            );
        }

        [Fact]
        public async Task CanSubmitAsync_WhenNoPreviousSubmission_ShouldReturnTrue()
        {
            var ipAddress = "192.168.1.1";
            _mockContactRepository
                .Setup(x => x.GetLastSubmissionTimeAsync(ipAddress))
                .ReturnsAsync(DateTime.MinValue);

            var result = await _rateLimitingService.CanSubmitAsync(ipAddress);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task CanSubmitAsync_WhenSubmittedWithinLimit_ShouldReturnFalse()
        {
            var ipAddress = "192.168.1.1";
            var now = DateTime.UtcNow;

            _mockContactRepository
                .Setup(x => x.GetLastSubmissionTimeAsync(ipAddress))
                .ReturnsAsync(now.AddSeconds(-30)); 

            var result = await _rateLimitingService.CanSubmitAsync(ipAddress);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task CanSubmitAsync_WhenSubmittedBeyondLimit_ShouldReturnTrue()
        {
            var ipAddress = "192.168.1.1";
            var now = DateTime.UtcNow;

            _mockContactRepository
                .Setup(x => x.GetLastSubmissionTimeAsync(ipAddress))
                .ReturnsAsync(now.AddSeconds(-61));

            var result = await _rateLimitingService.CanSubmitAsync(ipAddress);

            result.Should().BeTrue();
        }
    }
}
