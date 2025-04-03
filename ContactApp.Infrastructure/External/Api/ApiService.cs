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
        public ApiService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
        }
        public async Task<ContactDetailDto> GetAdditionalUserInfoAsync(string email)
        {
            try
            {
                string endpoint = $"{_apiSettings.BaseUrl}/{_apiSettings.UserEndpoint}?email={email}";
                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds);
                var response = await httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<List<ContactDetailDto>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (users == null || !users.Any())
                {
                    return null;
                }
                 var user = users[0];
                return user;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
