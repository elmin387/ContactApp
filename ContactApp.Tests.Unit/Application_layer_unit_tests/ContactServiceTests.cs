using ContactApp.Application.Dto;
using ContactApp.Application.Interfaces.Repositories;
using ContactApp.Application.Interfaces.Services;
using ContactApp.Application.Services;
using ContactApp.Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp.Tests.Unit.Application_layer_tests
{
    public class ContactServiceTests
    {
        private readonly Mock<IContactRepository> _mockContactRepository;
        private readonly Mock<IApiService> _mockApiService;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IRateLimitingService> _mockRateLimitingService;
        private readonly Mock<ILoggerService> _mockLoggerService;
        private readonly Application.Services.ContactService _contactService;

        public ContactServiceTests()
        {
            _mockContactRepository = new Mock<IContactRepository>();
            _mockApiService = new Mock<IApiService>();
            _mockEmailService = new Mock<IEmailService>();
            _mockRateLimitingService = new Mock<IRateLimitingService>();
            _mockLoggerService = new Mock<ILoggerService>();

            _contactService = new ContactService(
                _mockContactRepository.Object,
                _mockApiService.Object,
                _mockEmailService.Object,
                _mockRateLimitingService.Object,
                _mockLoggerService.Object
            );
        }

        [Fact]
        public async Task ProcessContactSubmissionAsync_WhenRateLimited_ShouldReturnFailure()
        {
            var contactDto = new ContactDto
            {
                Name = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };
            var ipAddress = "192.168.1.1";

            _mockRateLimitingService
                .Setup(x => x.CanSubmitAsync(ipAddress))
                .ReturnsAsync(false);

            var result = await _contactService.ProcessContactSubmissionAsync(contactDto, ipAddress);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Please wait");

            _mockContactRepository.Verify(x => x.CreateContactAsync(It.IsAny<Contact>()), Times.Never);
            _mockApiService.Verify(x => x.GetAdditionalUserInfoAsync(It.IsAny<string>()), Times.Never);
            _mockEmailService.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ContactDetailDto>()), Times.Never);
        }

        [Fact]
        public async Task ProcessContactSubmissionAsync_WhenSuccessful_ShouldReturnSuccess()
        {
            var contactDto = new ContactDto
            {
                Name = "Damir",
                LastName = "Donas",
                Email = "damir@lorem.de"
            };
            var ipAddress = "192.168.1.1";
            var contactId = 1;
            var additionalInfo = new ContactDetailDto
            {
                UserName = "damirDonas",
                Phone = "123456789",
                Website = "webiste.com",
                Address = new AddressDto
                {
                    Street = "Street 1",
                    Suite = "Apt 1",
                    City = "Berlin",
                    ZipCode = "10003",
                    Geo = new RegionDto { Lat = "40.7128", Lng = "-74.0060" }
                },
                Company = new CompanyDto
                {
                    Name = "Test Company",
                    CatchPhrase = "Making products",
                    Bs = "business test"
                }
            };

            _mockRateLimitingService
                .Setup(x => x.CanSubmitAsync(ipAddress))
                .ReturnsAsync(true);

            _mockContactRepository
                .Setup(x => x.CreateContactAsync(It.IsAny<Contact>()))
                .ReturnsAsync(contactId);

            _mockApiService
                .Setup(x => x.GetAdditionalUserInfoAsync(contactDto.Email))
                .ReturnsAsync(additionalInfo);

            // Act
            var result = await _contactService.ProcessContactSubmissionAsync(contactDto, ipAddress);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Contain("successfully");

            // Verify contact was created
            _mockContactRepository.Verify(x => x.CreateContactAsync(
                It.Is<Contact>(c =>
                    c.Name == contactDto.Name &&
                    c.LastName == contactDto.LastName &&
                    c.Email == contactDto.Email &&
                    c.IPAddress == ipAddress
                )),
                Times.Once);

            // Verify contact details were added
            _mockContactRepository.Verify(x => x.AddContactDetailsAsync(
                It.Is<int>(id => id == contactId),
                It.IsAny<ContactDetails>()
            ), Times.Once);

            // Verify email was sent
            _mockEmailService.Verify(x => x.SendEmailAsync(
                contactDto.Email,
                contactDto.Name,
                additionalInfo
            ), Times.Once);
        }

        [Fact]
        public async Task ProcessContactSubmissionAsync_WhenNoAdditionalInfo_ShouldStillSucceed()
        {
            // Arrange
            var contactDto = new ContactDto
            {
                Name = "Damir",
                LastName = "Donas",
                Email = "damir@lorem.de"
            };
            var ipAddress = "192.168.1.1";
            var contactId = 1;

            _mockRateLimitingService
                .Setup(x => x.CanSubmitAsync(ipAddress))
                .ReturnsAsync(true);

            _mockContactRepository
                .Setup(x => x.CreateContactAsync(It.IsAny<Contact>()))
                .ReturnsAsync(contactId);

            _mockApiService
                .Setup(x => x.GetAdditionalUserInfoAsync(contactDto.Email))
                .ReturnsAsync((ContactDetailDto)null);

            // Act
            var result = await _contactService.ProcessContactSubmissionAsync(contactDto, ipAddress);

            // Assert
            result.Success.Should().BeTrue();

            // Verify contact was created
            _mockContactRepository.Verify(x => x.CreateContactAsync(It.IsAny<Contact>()), Times.Once);

            // Verify contact details were NOT added
            _mockContactRepository.Verify(x => x.AddContactDetailsAsync(
                It.IsAny<int>(),
                It.IsAny<ContactDetails>()
            ), Times.Never);

            // Verify email was still sent
            _mockEmailService.Verify(x => x.SendEmailAsync(
                contactDto.Email,
                contactDto.Name,
                null
            ), Times.Once);
        }

        [Fact]
        public async Task ProcessContactSubmissionAsync_WhenExceptionOccurs_ShouldReturnFailure()
        {
            // Arrange
            var contactDto = new ContactDto
            {
                Name = "Damir",
                LastName = "Donas",
                Email = "damir@lorem.de"
            };
            var ipAddress = "192.168.1.1";

            _mockRateLimitingService
                .Setup(x => x.CanSubmitAsync(ipAddress))
                .ReturnsAsync(true);

            _mockContactRepository
                .Setup(x => x.CreateContactAsync(It.IsAny<Contact>()))
                .ThrowsAsync(new System.Exception("Database error"));

            // Act
            var result = await _contactService.ProcessContactSubmissionAsync(contactDto, ipAddress);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("error occurred");
        }
    }
}
