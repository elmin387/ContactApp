using ContactApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp.Application.Interfaces.Repositories
{
    public interface IContactRepository
    {
        Task<int> CreateContactAsync(Contact contact);
        Task<DateTime> GetLastSubmissionTimeAsync(string ipAddress);
        Task AddContactDetailsAsync(int contactId, ContactDetails contactDetails);
    }
}
