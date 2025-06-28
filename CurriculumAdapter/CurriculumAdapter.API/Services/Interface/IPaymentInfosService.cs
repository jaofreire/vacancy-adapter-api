using CurriculumAdapter.API.Models;
using CurriculumAdapter.API.Response;

namespace CurriculumAdapter.API.Services.Interface
{
    public interface IPaymentInfosService
    {
        Task<APIResponse<PaymentInfosModel>> GetByUserId(Guid userId);
        Task<APIResponse<PaymentInfosModel>> Update(Guid id, PaymentInfosModel model);
        Task<APIResponse<PaymentInfosModel>> Remove(Guid id);
    }
}
