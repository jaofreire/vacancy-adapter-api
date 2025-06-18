using CurriculumAdapter.API.Data.Integrations.Interfaces;
using CurriculumAdapter.API.Data.Integrations.Asaas.Request;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.JavaScript;
using CurriculumAdapter.API.Data.Integrations.Asaas.Response;

namespace CurriculumAdapter.API.Data.Integrations.Asaas
{
    public class AsaasIntegration(IConfiguration configuration) : IAsaasIntegration
    {
        private string _baseUrl = "https://api-sandbox.asaas.com/v3";
        private readonly IConfiguration _configuration = configuration;
        public async Task<CreateCustomerResponse?> CreateCustumer(CreateCustumerRequest request)
        {
            var url = $"{_baseUrl}/customers";
            var jsonRequestBody = JsonSerializer.Serialize(request);

            using var client = new HttpClient();

            var requestContent = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Headers =
                {
                    {"accept", "application/json"},
                    {"access_token", $"{_configuration["Asaas:ApiKey"] ?? Environment.GetEnvironmentVariable("ASAAS_API_KEY_PROD")}" }
                },
                Content = new StringContent(jsonRequestBody, new MediaTypeHeaderValue("application/json"))
            };

            var response = await client.SendAsync(requestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<CreateCustomerResponse>(responseContent);

                return responseObject;
            }

            return null;

        }

        public async Task<CreateSubscriptionWithCreditCardResponse?> CreateSubscription(CreateSubscriptionWithCreditCardRequest request)
        {
            var url = $"{_baseUrl}/subscriptions";
            var jsonRequestBody = JsonSerializer.Serialize(request);

            using var client = new HttpClient();

            var requestContent = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Headers =
                {
                    {"accept", "application/json"},
                    {"access_token", $"{_configuration["Asaas:ApiKey"] ?? Environment.GetEnvironmentVariable("ASAAS_API_KEY_PROD")}" }
                },
                Content = new StringContent(jsonRequestBody, new MediaTypeHeaderValue("application/json"))
            };

            var response = await client.SendAsync(requestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<CreateSubscriptionWithCreditCardResponse>(responseContent);

                return responseObject;
            }

            return null;
        }

        public async Task<UniquePaymentResponse?> UniquePayment()
        {
            var url = $"{_baseUrl}/subscriptions";

            var request = new UniquePaymentRequest();
            var jsonRequestBody = JsonSerializer.Serialize(request);

            using var client = new HttpClient();

            var requestContent = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Headers =
                {
                    {"accept", "application/json"},
                    {"access_token", $"{_configuration["Asaas:ApiKey"] ?? Environment.GetEnvironmentVariable("ASAAS_API_KEY_PROD")}" }
                },
                Content = new StringContent(jsonRequestBody, new MediaTypeHeaderValue("application/json"))
            };

            var response = await client.SendAsync(requestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<UniquePaymentResponse>(responseContent);

                return responseObject;
            }

            return null;
        }
    }
}
