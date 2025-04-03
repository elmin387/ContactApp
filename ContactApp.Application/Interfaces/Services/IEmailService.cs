using ContactApp.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp.Application.Interfaces.Services
{
    public interface IEmailService
    { 

        Task SendEmailAsync(string email, string name, ContactDetailDto? contactDetails);
    }
}
