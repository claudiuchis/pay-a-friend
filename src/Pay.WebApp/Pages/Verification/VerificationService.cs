using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using static Pay.WebApp.Commands;
using System.Text.Json;
using System.Text;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Extensions.Options;
using System.Net;

using Pay.WebApp.Configs;

namespace Pay.WebApp
{
    public class VerificationService
    {
        readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<ApiConfiguration> _apiConfig;

        public VerificationService(
            IOptions<ApiConfiguration> apiConfig,
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _apiConfig = apiConfig;
        }

        public async Task SendVerificationDetails(VerificationModel model)
        {
            var context = new HttpContextAccessor().HttpContext;
            var accessToken = await context.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var customerId = context.User.Claims.Where( claim => claim.Type == "sub").FirstOrDefault().Value;


            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // create the draft

            var detailsId = Guid.NewGuid().ToString();

            var draftCommand = new CreateDraftVerificationDetails {
                    VerificationDetailsId = detailsId,
                    CustomerId = customerId
            };
            var draftCommandJson = new StringContent(
                JsonSerializer.Serialize(draftCommand),
                Encoding.UTF8, 
                "application/json"
            );

            var response = await httpClient.PostAsync("/api/verify/draft", draftCommandJson);

            if (response.StatusCode != HttpStatusCode.OK) throw new Exception($"{response.StatusCode} {response.ReasonPhrase}");

            // add date of birth

            var dobCommand = new AddDateOfBirth {
                VerificationDetailsId = detailsId,
                DateOfBirth = model.DateOfBirth
            };
            var dobCommandJson = new StringContent(
                JsonSerializer.Serialize(dobCommand),
                Encoding.UTF8,
                "application/json"
            );
            response = await httpClient.PostAsync("/api/verify/dob", dobCommandJson);
            if (response.StatusCode != HttpStatusCode.OK) throw new Exception($"{response.StatusCode} {response.ReasonPhrase}");

            // add the address

            var addressCommand = new AddAddress {
                VerificationDetailsId = detailsId,
                Address1 = model.Address1,
                Address2 = model.Address2,
                CityTown = model.CityTown,
                CountyState = model.CountyState,
                Code = model.Code,
                Country = model.Country
            };
            var addressCommandJson = new StringContent(
                JsonSerializer.Serialize(addressCommand),
                Encoding.UTF8,
                "application/json"
            );
            response = await httpClient.PostAsync("/api/verify/address", addressCommandJson);
            if (response.StatusCode != HttpStatusCode.OK) throw new Exception($"{response.StatusCode} {response.ReasonPhrase}");

            // submit the details

            var submitCommand = new SubmitDetails {
                VerificationDetailsId = detailsId
            };
            var submitCommandJson = new StringContent(
                JsonSerializer.Serialize(submitCommand),
                Encoding.UTF8,
                "application/json"
            );
            response = await httpClient.PostAsync("/api/verify/submit", submitCommandJson);
            if (response.StatusCode != HttpStatusCode.OK) throw new Exception($"{response.StatusCode} {response.ReasonPhrase}");

        }
    }
}