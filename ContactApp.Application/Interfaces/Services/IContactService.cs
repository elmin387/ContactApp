using ContactApp.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp.Application.Interfaces.Services
{
    public interface IContactService
    {
        Task<(bool Success, string Message)> ProcessContactSubmissionAsync(ContactDto contactDto, string ipAddress);
    }
}
