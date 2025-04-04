using ContactApp.Application.Interfaces.Repositories;
using ContactApp.Application.Interfaces.Services;
using ContactApp.Infrastructure.Configuration;
using ContactApp.Infrastructure.Data.Repositories;
using ContactApp.Infrastructure.Data;
using ContactApp.Infrastructure.External.Api;
using ContactApp.Infrastructure.External.Email;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ContactApp.Infrastructure.Logger;

namespace ContactApp.Infrastructure
{
    public static class DependencyInjectionContainer
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<Application.Settings.RateLimitSettings>(configuration.GetSection("RateLimitSettings"));

            
            services.AddSingleton<IConnectionFactory>(provider =>
                new ConnectionFactory(configuration.GetConnectionString("DefaultConnection")));

            
            services.AddScoped<IContactRepository, ContactRepository>();

            
            services.AddScoped<IApiService, ApiService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddSingleton<ILoggerService, SerilogLoggerService>();


            services.AddHttpClient();

            return services;
        }
    }
}
