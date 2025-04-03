using ContactApp.Application.Dto;

namespace ContactApp.Application.Interfaces.Services
{
    public interface IApiService
    {
        Task<ContactDetailDto> GetAdditionalUserInfoAsync(string email);
    }
}
