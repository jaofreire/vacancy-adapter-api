using CurriculumAdapter.API.Data.Repositories.Interfaces;
using CurriculumAdapter.API.Models;
using CurriculumAdapter.API.Response;
using CurriculumAdapter.API.Services.Interface;

namespace CurriculumAdapter.API.Services
{
    public class PaymentInfosService(IUnitOfWork unitOfWork) : IPaymentInfosService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<APIResponse<PaymentInfosModel>> GetByUserId(Guid userId)
        {
            var paymentInfos = await _unitOfWork.PaymentInfosRepository.Get(x => x.UserId == userId);

            return new APIResponse<PaymentInfosModel>(true, 200, "Dados de pagamento listado com sucesso", null, paymentInfos);
        }

        public async Task<APIResponse<PaymentInfosModel>> Remove(Guid id)
        {
            var paymentInfo = await _unitOfWork.PaymentInfosRepository.GetById(id);

            if(paymentInfo is null)
                return new APIResponse<PaymentInfosModel>(false, 404, "Dados de pagemento não encontrado");

            await _unitOfWork.BeginTransaction();

            _unitOfWork.PaymentInfosRepository.Delete(paymentInfo);
            await _unitOfWork.Commit();

            return new APIResponse<PaymentInfosModel>(true, 200, "Dados de pagemento removidos com sucesso");
        }

        public async Task<APIResponse<PaymentInfosModel>> Update(Guid id, PaymentInfosModel model)
        {
            return new APIResponse<PaymentInfosModel>(true, 200, "Endpoint em implementação");
        }
    }
}
