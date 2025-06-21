using CurriculumAdapter.API.Data.Integrations.Asaas.Request;
using CurriculumAdapter.API.Data.Integrations.Asaas.Response;

namespace CurriculumAdapter.API.Data.Integrations.Interfaces
{
    public interface IAsaasIntegration
    {
        Task<CreateSubscriptionWithCreditCardResponse?> CreateSubscription(CreateSubscriptionWithCreditCardRequest request);
        Task<bool> GetCustomerById(string customerId);
        Task<UniquePaymentResponse?> UniquePayment();
        Task<CreateCustomerResponse?> CreateCustumer(CreateCustumerRequest request);
    }
}
