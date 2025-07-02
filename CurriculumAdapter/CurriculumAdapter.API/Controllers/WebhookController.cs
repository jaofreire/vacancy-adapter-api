using CurriculumAdapter.API.Data.Integrations.Interfaces;
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
    public class WebhookController(IUnitOfWork unitOfWork, IAsaasIntegration asaasIntegration) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAsaasIntegration _asaasIntegration = asaasIntegration;

        [HttpPost("billing-status-update")]
        public async Task<ActionResult<APIResponse<string>>> BillingStatusUpdate(WebhookPaymentEventInputDTO eventInput)
        {
            if(eventInput.Event == PaymentEventEnum.PAYMENT_CONFIRMED.ToString())
            {
                string? subscriptionId = eventInput.Payment.Subscription;
                if (!string.IsNullOrEmpty(subscriptionId))
                {
                    var existsSubscriptionUser = await _unitOfWork.UserRepository.Get(x => x.CurrentSubscriptionId == subscriptionId);

                    if (!existsSubscriptionUser.Any())
                        return NotFound(new APIResponse<string>(false, 404, "Nenhum usuário pertencente á assinatura foi encontrado"));

                    var subscriptionUser = existsSubscriptionUser.First();


                    var subscriptionsByCustomerId = await _asaasIntegration.GetSubscriptionsByCustomerId(eventInput.Payment.Customer);

                    if (subscriptionsByCustomerId is null)
                        return BadRequest(new APIResponse<string>(false, 400, "Ocorreu um problema ao buscar Assinaturas do usuário"));

                    var currentSubscription = subscriptionsByCustomerId.data.FirstOrDefault(x => x.id == subscriptionId);

                    if (currentSubscription is null)
                        return NotFound(new APIResponse<string>(false, 404, "Ocorreu um problema ao buscar Assinatura Atual do usuário"));

                    await _unitOfWork.BeginTransaction();


                    if (subscriptionUser.Type == UserTypeEnum.Subscriber)
                    {
                        subscriptionUser.SubscriptionEndDate = currentSubscription.nextDueDate.ToString("dd/MM/yyyy");

                        _unitOfWork.UserRepository.Update(subscriptionUser);
                        await _unitOfWork.Commit();

                        return Ok(new APIResponse<string>(true, 200, "Assinatura renovada com sucesso"));
                    }

                    subscriptionUser.Type = UserTypeEnum.Subscriber;
                    subscriptionUser.SubscriptionEndDate = currentSubscription.nextDueDate.ToString("dd/MM/yyyy");

                    _unitOfWork.UserRepository.Update(subscriptionUser);
                    await _unitOfWork.Commit();

                    return Ok(new APIResponse<string>(true, 200, "Pagamento da assinatura confirmado, usuário agora é Assinante"));
                }

                //Lógica para Pagamentos únicos
                var userExists = await _unitOfWork.UserRepository.Get(x => x.AsaasCustomerId == eventInput.Payment.Customer);

                if (!userExists.Any())
                    return NotFound(new APIResponse<string>(false, 404, "Usuário não encontrado"));

                var user = userExists.First();

                user.Type = UserTypeEnum.Subscriber;

                //Atualizar SubscriptionEndDate para o mesmo dia no mes seguinte

                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.Commit();

                return Ok(new APIResponse<string>(true, 200, "Pagamento confirmado e usuário atualizado com sucesso"));
            } 

            return Ok(new APIResponse<string>(true, 200, "Webhook recebido porém pagamento ainda não foi confirmado"));
        }

        [HttpPost("subscription-status-update")]
        public async Task<ActionResult<APIResponse<string>>> SubscriptionStatusUpdate(WebhookSubscriptionEventInputDTO eventInput)
        {
            if(eventInput.Event == SubscriptionEventEnum.SUBSCRIPTION_CREATED.ToString())
            {
                var userExists = await _unitOfWork.UserRepository.Get(x => x.AsaasCustomerId == eventInput.Subscription.Customer);

                if (!userExists.Any())
                    return NotFound(new APIResponse<string>(false, 404, "Usuário não encontrado"));

                var user = userExists.First();

                await _unitOfWork.BeginTransaction();

                user.CurrentSubscriptionId = eventInput.Subscription.Id;

                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.Commit();

                return Ok(new APIResponse<string>(true, 200, "Assinatura criada com sucesso"));
            }

            if(eventInput.Event == SubscriptionEventEnum.SUBSCRIPTION_DELETED.ToString())
            {
                var userExists = await _unitOfWork.UserRepository.Get(x => x.AsaasCustomerId == eventInput.Subscription.Customer);

                if (!userExists.Any())
                    return NotFound(new APIResponse<string>(false, 404, "Usuário não encontrado"));

                var user = userExists.First();

                await _unitOfWork.BeginTransaction();

                user.Type = UserTypeEnum.Default;

                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.Commit();

                return Ok(new APIResponse<string>(true, 200, "Assinatura removida com sucesso"));
            }

            return Ok( new APIResponse<string>(true, 200, "Webhook de assinatura recebido com sucesso"));
        }

    }
}
