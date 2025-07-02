using CurriculumAdapter.API.Data.Integrations.Asaas.Response;
using CurriculumAdapter.API.DTOs;
using CurriculumAdapter.API.Response;

namespace CurriculumAdapter.API.Services.Interface
{
    public interface IPaymentService
    {
        Task<APIResponse<CreateSubscriptionWithCreditCardResponse>> CreateSubscription(CreateSubscriptionInputDTO input);
        Task<APIResponse<CreateSubscriptionWithCreditCardResponse>> CreateSubscriptionByPaymentInfoId(Guid id, CreditCardInputDTO input);
        Task GetSubscriptionById(Guid id);
        Task<APIResponse<UniquePaymentResponse>> GenerateUniquePayment(GenerateUniquePaymentInputDTO input);
        Task<APIResponse<UniquePaymentResponse>> GenerateUniquePaymentWithPaymentInfoId(Guid paymentInfoId);
        Task<APIResponse<GetSubscriptionsByCustomerIdResponse>> GetSubscriptionsByCustomerId(string customerId);
        Task<APIResponse<GetPaymentsBySubscriptionIdResponse>> GetPaymentsBySubscriptionId(string subscriptionId);
        Task<APIResponse<GetUniquePaymentsByCustomerIdResponse>> GetUniquePaymentsByCustomerId(string customerId);
        Task<APIResponse<string>> RemoveSubscription(string subscriptionId);

    }
}
