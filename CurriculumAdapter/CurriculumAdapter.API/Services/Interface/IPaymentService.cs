using CurriculumAdapter.API.Data.Integrations.Asaas.Response;
using CurriculumAdapter.API.DTOs;
using CurriculumAdapter.API.Response;

namespace CurriculumAdapter.API.Services.Interface
{
    public interface IPaymentService
    {
        Task<APIResponse<CreateSubscriptionWithCreditCardResponse>> CreateSubscription(CreateSubscriptionInputDTO input);
        Task CreateSubscriptionByPaymentInfoId(Guid id);
        Task GetSubscriptionById(Guid id);
    }
}
