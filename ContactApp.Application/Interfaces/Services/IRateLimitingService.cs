using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp.Application.Interfaces.Services
{
    public interface IRateLimitingService
    {
        Task<bool> CanSubmitAsync(string ipAddress);
    }
}
