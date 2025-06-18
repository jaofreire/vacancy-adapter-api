using CurriculumAdapter.API.Data.Integrations.Asaas.Request;
using CurriculumAdapter.API.Data.Integrations.Asaas.Response;

namespace CurriculumAdapter.API.Data.Integrations.Interfaces
{
    public interface IAsaasIntegration
    {
        Task<string> CreateSubscription();
        Task<string> UniquePayment();
        Task<CreateCustomerResponse?> CreateCustumer(CreateCustumerRequest request);
    }
}
