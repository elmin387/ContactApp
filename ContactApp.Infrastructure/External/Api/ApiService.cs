using ContactApp.Application.Dto;
using ContactApp.Application.Interfaces.Services;
using ContactApp.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContactApp.Infrastructure.External.Api
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
        private readonly ILoggerService _logger;

        public ApiService(HttpClient httpClient, IOptions<ApiSettings> apiSettings, ILoggerService loggerService)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
            _logger = loggerService;
        }
        public async Task<ContactDetailDto> GetAdditionalUserInfoAsync(string email)
        {
            try
            {
                string endpoint = $"{_apiSettings.BaseUrl}/{_apiSettings.UserEndpoint}?email={email}";
                _logger.LogInformation("Retrieving additional contact details for email: {Email}, Endpoint: {Endpoint}", email, endpoint);
                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds);
                var response = await httpClient.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("API response is not suuccess. Status: {StatusCode}, Email: {Email}",
                        response.StatusCode, email);
                    return null;
                }
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogDebug("Received API response for email: {Email}, Content length: {ContentLength}",
                    email, content.Length);
                var users = JsonSerializer.Deserialize<List<ContactDetailDto>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (users == null || !users.Any())
                {
                    _logger.LogWarning("API does not return the contact details for email: {Email}", email);
                    return null;
                }
                 var user = users[0];
                _logger.LogInformation("Retrieved additional informations for the user: {UserName}, Email: {Email}",
                    user.UserName, email);
                return user;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There is error in retrieving additional informations: {Email}", email);
                return null;
            }
        }
    }
}
