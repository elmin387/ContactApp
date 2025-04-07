using ContactApp.Application.Dto;

namespace ContactApp.Application.Interfaces.Services
{
    public interface IEmailService
    { 

        Task SendEmailAsync(string email, string name, ContactDetailDto? contactDetails);
    }
}
