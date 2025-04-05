using ContactApp.Application.Dto;
using ContactApp.Application.Interfaces.Repositories;
using ContactApp.Application.Interfaces.Services;
using ContactApp.Domain.Entities;

namespace ContactApp.Application.Services
{
    public class ContactService: IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IApiService _apiService;
        private readonly IEmailService _emailService;
        private readonly IRateLimitingService _rateLimitingService;
        private readonly ILoggerService _logger;

        public ContactService(IContactRepository contactRepository,
            IApiService apiService,
            IEmailService emailService,
            IRateLimitingService rateLimitingService, ILoggerService loggerService)
        {
            _contactRepository = contactRepository;
            _apiService = apiService;
            _emailService = emailService;
            _rateLimitingService = rateLimitingService;
            _logger = loggerService;
        }

        public async Task<(bool Success, string Message)> ProcessContactSubmissionAsync(ContactDto contactDto, string ipAddress)
        {
            try
            {
                _logger.LogInformation("Start processing contact for email: {@Email}, IP: {@IPAddress}}", contactDto.Email,ipAddress);
                bool canSubmit = await _rateLimitingService.CanSubmitAsync(ipAddress);
                if (!canSubmit)
                {
                    _logger.LogWarning("To much request in short time from ipAddress: {IPAddress}", ipAddress);
                    return (false, "Please wait at least one minute between submissions.");
                }
                var contact = new Contact
                {
                    Name = contactDto.Name,
                    LastName = contactDto.LastName,
                    Email = contactDto.Email,
                    IPAddress = ipAddress
                };
                _logger.LogDebug("Creating contact in db: {Name} {LastName}, {Email}", contactDto.Name, contactDto.LastName, contactDto.Email);
                int contactId = await _contactRepository.CreateContactAsync(contact);
                _logger.LogInformation("Contact created successfully with ID: {ContactId}", contactId);
                var additionalInfo = await _apiService.GetAdditionalUserInfoAsync(contactDto.Email);
                if (additionalInfo != null)
                {

                    _logger.LogInformation("Retrieved additional information: {ContactId}", contactId);
                    var contactDetails = MapToContactDetails(contactId, additionalInfo);

                   
                    await _contactRepository.AddContactDetailsAsync(contactId, contactDetails);
                    _logger.LogInformation("Saved additional contact details to db for contact: {ContactId}", contactId);

                }
                else
                {
                    _logger.LogWarning("There is no additional contact details for email: {Email}", contactDto.Email);
                }
                await _emailService.SendEmailAsync(contactDto.Email, contactDto.Name, additionalInfo);
                _logger.LogInformation("Email sent for contact: {ContactId}", contactId);
                return (true, "Contact information successfully submitted!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Contact processing error for email: {Email}", contactDto.Email);
                return (false, "An error occurred while processing your submission. Please try again later.");
            }   
        }

            private ContactDetails MapToContactDetails(int contactId, ContactDetailDto dto)
            {
              _logger.LogDebug("Mapping additional details for contact: {ContactId}", contactId);
            return new ContactDetails
                {
                    ContactDetailsId = contactId,
                    UserName = dto.UserName,
                    Phone = dto.Phone,
                    WebSite = dto.Website,
                    Address = dto.Address != null
                        ? new Address
                        {
                            Street = dto.Address.Street,
                            Suite = dto.Address.Suite,
                            City = dto.Address.City,
                            ZipCode = dto.Address.ZipCode,
                            Geo = dto.Address.Geo != null
                                ? new Region
                                {
                                    Lat = dto.Address.Geo.Lat,
                                    Lng = dto.Address.Geo.Lng
                                }
                                : null
                        }
                        : null,
                    Company = dto.Company != null
                        ? new Company
                        {
                            CompanyName = dto.Company.Name,
                            CatchPhrase = dto.Company.CatchPhrase,
                            Bs = dto.Company.Bs
                        }
                        : null
                };
            }
    }
}
