using CurriculumAdapter.API.Data.Repositories.Interfaces;
using CurriculumAdapter.API.DTOs;
using CurriculumAdapter.API.Models.Enums;
using CurriculumAdapter.API.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurriculumAdapter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [HttpPost("billing-status-update")]
        public async Task<ActionResult<APIResponse<string>>> BillingStatusUpdate(WebhookEventInputDTO eventInput)
        {
            if(eventInput.Event == PaymentEventEnum.PAYMENT_CONFIRMED.ToString())
            {
                var userExists = await _unitOfWork.UserRepository.Get(x => x.AsaasCustomerId == eventInput.Payment.Customer);

                if (!userExists.Any())
                    return NotFound(new APIResponse<string>(false, 404, "usuário não encontrado"));

                var user = userExists.First();

                user.Type = UserTypeEnum.Subscriber;

                await _unitOfWork.BeginTransaction();

                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.Commit();

                return Ok(new APIResponse<string>(true, 200, "Pagamento confirmado e usuário atualizado com sucesso"));
            }

            //identificar caso seja um evento sobre cobrança de assinatura e implementar lógica

            return Ok(new APIResponse<string>(true, 200, "Webhook recebido porém pagamento ainda não foi confirmado"));
        }

        [HttpPost("subscription-status-update")]
        public async Task<ActionResult<APIResponse<string>>> SubscriptionStatusUpdate(WebhookEventInputDTO eventInput)
        {
            if(eventInput.Event == SubscriptionEventEnum.SUBSCRIPTION_DELETED.ToString())
            {
                var userExists = await _unitOfWork.UserRepository.Get(x => x.AsaasCustomerId == eventInput.Payment.Customer);

                if (!userExists.Any())
                    return new APIResponse<string>(false, 404, "Usuário não encontrado");

                var user = userExists.First();

                await _unitOfWork.BeginTransaction();

                user.Type = UserTypeEnum.Default;

                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.Commit();

                return new APIResponse<string>(true, 200, "Assinatura removida com sucesso");
            }

            return new APIResponse<string>(true, 200, "Webhook de assinatura recebido com sucesso");
        }

    }
}
