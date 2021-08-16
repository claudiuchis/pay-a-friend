using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net;
using Newtonsoft.Json;

using Pay.WebApp.Configs;

namespace Pay.WebApp.Pages.Home
{
    public class CustomersService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<ApiConfiguration> _apiConfig;

        public CustomersService(
            IHttpClientFactory httpClientFactory,
            IOptions<ApiConfiguration> apiConfig,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _httpClientFactory = httpClientFactory;
            _apiConfig = apiConfig;
        }

        public async Task<CustomerModel> GetCustomerById(string customerId)
        {
            CustomerModel customer = null;

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_apiConfig.Value.VerificationAPI);

            var context = new HttpContextAccessor().HttpContext;
            var accessToken = await context.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetAsync($"/api/customers/{customerId}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                customer = JsonConvert.DeserializeObject<CustomerModel>(result);
            }
            return customer;
        }
    }
}