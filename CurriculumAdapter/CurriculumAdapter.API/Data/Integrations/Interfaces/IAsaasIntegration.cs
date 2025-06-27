using CurriculumAdapter.API.Data.Integrations.Asaas.Request;
using CurriculumAdapter.API.Data.Integrations.Asaas.Response;

namespace CurriculumAdapter.API.Data.Integrations.Interfaces
{
    public interface IAsaasIntegration
    {
        Task<CreateSubscriptionWithCreditCardResponse?> CreateSubscription(CreateSubscriptionWithCreditCardRequest request);
        Task<bool> GetCustomerById(string customerId);
        Task<GetSubscriptionsByCustomerIdResponse?> GetSubscriptionsByCustomerId(string customerId);
        Task<GetPaymentsBySubscriptionIdResponse?> GetPaymentsBySubscriptionId(string subscriptionId);
        Task<GetUniquePaymentsByCustomerIdResponse?> GetUniquePaymentsByCustomerId(string customerId);
        Task<UniquePaymentResponse?> UniquePayment();
        Task<CreateCustomerResponse?> CreateCustumer(CreateCustumerRequest request);
    }
}
