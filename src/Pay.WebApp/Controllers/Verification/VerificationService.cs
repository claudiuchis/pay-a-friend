using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using static Pay.WebApp.Commands;
using System.Text.Json;
using System.Text;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Extensions.Options;

namespace Pay.WebApp
{
    public class VerificationService
    {
        private HttpClient _httpClient;


        public VerificationService(
            IOptions<ApiConfiguration> apiConfig,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(apiConfig.Value.VerificationAPI);
        }

        public async Task CreateDraftVerificationDetails(CreateDraftVerificationDetails command)
        {
            var context = new HttpContextAccessor().HttpContext;
            var accessToken = await context.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var commandJson = new StringContent(
                JsonSerializer.Serialize(command),
                Encoding.UTF8, 
                "application/json"
            );
            var response = await _httpClient.PostAsync("/api/verify/draft", commandJson);
        }
    }
}