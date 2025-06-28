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
        private string _baseUrl = "https://api.asaas.com/v3";
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
                    {"access_token", $"{_configuration["Asaas:ApiKey"] ?? Environment.GetEnvironmentVariable("ASAAS_API_KEY_PROD")}" },
                    {"User-Agent", "CurriculumAdapter"}

                },
                Content = new StringContent(jsonRequestBody, new MediaTypeHeaderValue("application/json"))
            };

            var response = await client.SendAsync(requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                
                var responseObject = JsonSerializer.Deserialize<CreateCustomerResponse>(responseContent);

                return responseObject;
            }

            return null;

        }

        public async Task<GetAllCustomersResponse?> GetAllCustomers()
        {
            var url = $"{_baseUrl}/customers";

            using var client = new HttpClient();

            var requestContent = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                    {"accept", "application/json"},
                    {"access_token", $"{_configuration["Asaas:ApiKey"] ?? Environment.GetEnvironmentVariable("ASAAS_API_KEY_PROD")}" },
                    {"User-Agent", "CurriculumAdapter"}
                }
            };

            var response = await client.SendAsync(requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseObject = JsonSerializer.Deserialize<GetAllCustomersResponse>(responseContent);

                return responseObject;
            }

            return null;
        }

        public async Task<bool> GetCustomerById(string customerId)
        {
            var url = $"{_baseUrl}/customers/{customerId}";

            using var client = new HttpClient();

            var requestContent = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                    {"accept", "application/json"},
                    {"access_token", $"{_configuration["Asaas:ApiKey"] ?? Environment.GetEnvironmentVariable("ASAAS_API_KEY_PROD")}" },
                    {"User-Agent", "CurriculumAdapter"}
                }
            };

            var response = await client.SendAsync(requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }
        
        public async Task<GetSubscriptionsByCustomerIdResponse?> GetSubscriptionsByCustomerId(string customerId)
        {
            var url = $"{_baseUrl}/subscriptions?customer={customerId}";

            using var client = new HttpClient();

            var requestContent = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                    {"accept", "application/json"},
                    {"access_token", $"{_configuration["Asaas:ApiKey"] ?? Environment.GetEnvironmentVariable("ASAAS_API_KEY_PROD")}" },
                    {"User-Agent", "CurriculumAdapter"}

                },
            };

            var response = await client.SendAsync(requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseObject = JsonSerializer.Deserialize<GetSubscriptionsByCustomerIdResponse>(responseContent);

                return responseObject;
            }

            return null;

        }
        
        public async Task<GetPaymentsBySubscriptionIdResponse?> GetPaymentsBySubscriptionId(string subscriptionId)
        {
            var url = $"{_baseUrl}/subscriptions/{subscriptionId}/payments";

            using var client = new HttpClient();

            var requestContent = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                    {"accept", "application/json"},
                    {"access_token", $"{_configuration["Asaas:ApiKey"] ?? Environment.GetEnvironmentVariable("ASAAS_API_KEY_PROD")}" },
                    {"User-Agent", "CurriculumAdapter"}

                },
            };

            var response = await client.SendAsync(requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseObject = JsonSerializer.Deserialize<GetPaymentsBySubscriptionIdResponse>(responseContent);

                return responseObject;
            }

            return null;
        }

        public async Task<GetUniquePaymentsByCustomerIdResponse?> GetUniquePaymentsByCustomerId(string customerId)
        {
            var url = $"{_baseUrl}/payments?customer={customerId}&externalReference=unique-payment";

            using var client = new HttpClient();

            var requestContent = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                    {"accept", "application/json"},
                    {"access_token", $"{_configuration["Asaas:ApiKey"] ?? Environment.GetEnvironmentVariable("ASAAS_API_KEY_PROD")}" },
                    {"User-Agent", "CurriculumAdapter"}

                },
            };

            var response = await client.SendAsync(requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseObject = JsonSerializer.Deserialize<GetUniquePaymentsByCustomerIdResponse>(responseContent);

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
                    {"access_token", $"{_configuration["Asaas:ApiKey"] ?? Environment.GetEnvironmentVariable("ASAAS_API_KEY_PROD")}" },
                    {"User-Agent", "CurriculumAdapter"}
                },
                Content = new StringContent(jsonRequestBody, new MediaTypeHeaderValue("application/json"))
            };

            var response = await client.SendAsync(requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseObject = JsonSerializer.Deserialize<CreateSubscriptionWithCreditCardResponse>(responseContent);

                return responseObject;
            }

            return null;
        }

        public async Task<bool> RemoveSubscription(string subscriptionId)
        {
            var url = $"{_baseUrl}/subscriptions/{subscriptionId}";

            using var client = new HttpClient();

            var requestContent = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(url),
                Headers =
                {
                    {"accept", "application/json"},
                    {"access_token", $"{_configuration["Asaas:ApiKey"] ?? Environment.GetEnvironmentVariable("ASAAS_API_KEY_PROD")}" },
                    {"User-Agent", "CurriculumAdapter"}
                },
            };

            var response = await client.SendAsync(requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }

        public async Task<UniquePaymentResponse?> UniquePayment(UniquePaymentRequest request)
        {
            var url = $"{_baseUrl}/payments";
            var jsonRequestBody = JsonSerializer.Serialize(request);

            using var client = new HttpClient();

            var requestContent = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Headers =
                {
                    {"accept", "application/json"},
                    {"access_token", $"{_configuration["Asaas:ApiKey"] ?? Environment.GetEnvironmentVariable("ASAAS_API_KEY_PROD")}" },
                    {"User-Agent", "CurriculumAdapter"}
                },
                Content = new StringContent(jsonRequestBody, new MediaTypeHeaderValue("application/json"))
            };

            var response = await client.SendAsync(requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseObject = JsonSerializer.Deserialize<UniquePaymentResponse>(responseContent);

                return responseObject;
            }

            return null;
        }

    }
}
