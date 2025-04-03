using ContactApp.Application.Interfaces.Services;
using ContactApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp.Application
{
    public static class DependencyInjectionContainer
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IRateLimitingService, RateLimitingService>();
            return services;
        }
    }
}
