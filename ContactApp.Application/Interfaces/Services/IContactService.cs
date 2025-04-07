using ContactApp.Application.Dto;

namespace ContactApp.Application.Interfaces.Services
{
    public interface IContactService
    {
        Task<(bool Success, string Message)> ProcessContactSubmissionAsync(ContactDto contactDto, string ipAddress);
    }
}
