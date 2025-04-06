using ContactApp.Application.Interfaces.Services;
using ContactApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

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
