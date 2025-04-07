using ContactApp.Domain.Entities;

namespace ContactApp.Application.Interfaces.Repositories
{
    public interface IContactRepository
    {
        Task<int> CreateContactAsync(Contact contact);
        Task<DateTime> GetLastSubmissionTimeAsync(string ipAddress);
        Task AddContactDetailsAsync(int contactId, ContactDetails contactDetails);
    }
}
