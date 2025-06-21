using CurriculumAdapter.API.Data.Integrations.Asaas.Response;
using CurriculumAdapter.API.Data.Integrations.Interfaces;
using CurriculumAdapter.API.DTOs;
using CurriculumAdapter.API.Response;
using CurriculumAdapter.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurriculumAdapter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IAsaasIntegration asaasIntegration, IPaymentService paymentService) : ControllerBase
    {
        private readonly IAsaasIntegration _asaasIntegration = asaasIntegration;
        private readonly IPaymentService _paymentService = paymentService;

        [HttpGet("generate-payment-link")]
        [Authorize("EveryoneHasAccessPolicy")]
        public async Task<ActionResult<APIResponse<UniquePaymentResponse>>> GeneratePaymentLink()
        {
            var uniquePaymentResponse = await _asaasIntegration.UniquePayment();

            if (uniquePaymentResponse is null)
                return BadRequest(new APIResponse<UniquePaymentResponse>(false, 400, "Ocorreu um erro ao gerar link de pagamento unico"));

            return Ok(new APIResponse<UniquePaymentResponse>(true, 200, "Link de pagamento unico gerado com sucesso", uniquePaymentResponse, null));
        }

        [HttpPost("create-subscription-with-credit-card")]
        [Authorize("EveryoneHasAccessPolicy")]
        public async Task<ActionResult<APIResponse<CreateSubscriptionWithCreditCardResponse>>> CreateSubscriptionWithCreditCard(CreateSubscriptionInputDTO input)
        {
            var response = await _paymentService.CreateSubscription(input);

            if (response.Code is not 200)
                return BadRequest(response);

            return Ok(response);
        }

    }
}
