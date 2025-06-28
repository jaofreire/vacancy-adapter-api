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

        [HttpGet("customers")]
        public async Task<ActionResult<APIResponse<GetAllCustomersResponse>>> GetAllCustomers()
        {
            var response = await _asaasIntegration.GetAllCustomers();

            if (response is null)
                return BadRequest(new APIResponse<GetAllCustomersResponse>(false, 400, "Ocorreu um erro ao listar todos os Cliente"));

            return Ok(new APIResponse<GetAllCustomersResponse>(true, 200, "Clientes listados com sucesso", response, null));
        }

        [HttpPost("generate-unique-payment")]
        [Authorize("EveryoneHasAccessPolicy")]
        public async Task<ActionResult<APIResponse<UniquePaymentResponse>>> GenerateUniquePayment(GenerateUniquePaymentInputDTO input)
        {
            var uniquePaymentResponse = await _paymentService.GenerateUniquePayment(input);

            if (uniquePaymentResponse.Code is not 200)
                return BadRequest(uniquePaymentResponse);

            return Ok(uniquePaymentResponse);
        }

        [HttpGet("generate-unique-payment/paymentInfo/{paymentInfoId}")]
        [Authorize("EveryoneHasAccessPolicy")]
        public async Task<ActionResult<APIResponse<UniquePaymentResponse>>> GenerateUniquePaymentWithPaymentInfoId(Guid paymentInfoId)
        {
            var uniquePaymentResponse = await _paymentService.GenerateUniquePaymentWithPaymentInfoId(paymentInfoId);

            if (uniquePaymentResponse.Code is 404)
                return NotFound(uniquePaymentResponse);

            if(uniquePaymentResponse.Code is 400)
                return BadRequest(uniquePaymentResponse);

            return Ok(uniquePaymentResponse);
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

        [HttpGet("subscriptions/customer/{customerId}")]
        [Authorize("EveryoneHasAccessPolicy")]
        public async Task<ActionResult<APIResponse<GetSubscriptionsByCustomerIdResponse>>> GetSubscriptionsByCustomerId(string customerId)
        {
            var response = await _paymentService.GetSubscriptionsByCustomerId(customerId);

            if (response.Code is not 200)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("payments/subscription/{subscriptionId}")]
        [Authorize("EveryoneHasAccessPolicy")]
        public async Task<ActionResult<APIResponse<GetPaymentsBySubscriptionIdResponse>>> GetPaymentsBySubscriptionId(string subscriptionId)
        {
            var response = await _paymentService.GetPaymentsBySubscriptionId(subscriptionId);

            if (response.Code is not 200)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpGet("unique-payments/customer/{customerId}")]
        [Authorize("EveryoneHasAccessPolicy")]
        public async Task<ActionResult<APIResponse<GetUniquePaymentsByCustomerIdResponse>>> GetUniquePaymentsByCustomerId(string customerId)
        {
            var response = await _paymentService.GetUniquePaymentsByCustomerId(customerId);

            if (response.Code is not 200)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("subscription/{subscriptionId}")]
        [Authorize("SubscriberPolicy")]
        public async Task<ActionResult<APIResponse<bool>>> RemoveSubscription(string subscriptionId)
        {
            var response = await _paymentService.RemoveSubscription(subscriptionId);

            if(response.Code is 400)
                return BadRequest(response);

            if(response.Code is 404)
                return NotFound(response);

            return Ok(response);
        }

    }
}
