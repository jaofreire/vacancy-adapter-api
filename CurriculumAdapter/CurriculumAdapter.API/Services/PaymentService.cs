using CurriculumAdapter.API.Data.Integrations.Asaas.Request;
using CurriculumAdapter.API.Data.Integrations.Asaas.Response;
using CurriculumAdapter.API.Data.Integrations.Interfaces;
using CurriculumAdapter.API.Data.Repositories.Interfaces;
using CurriculumAdapter.API.DTOs;
using CurriculumAdapter.API.Models;
using CurriculumAdapter.API.Response;
using CurriculumAdapter.API.Services.Interface;
using System.Security.Claims;
using CurriculumAdapter.API.Models.Enums;

namespace CurriculumAdapter.API.Services
{
    public class PaymentService(
        IHttpContextAccessor httpContextAccessor,
        IAsaasIntegration asaasIntegration,
        IUnitOfWork unitOfWork) : IPaymentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IAsaasIntegration _asaasIntegration = asaasIntegration;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<APIResponse<CreateSubscriptionWithCreditCardResponse>> CreateSubscription(CreateSubscriptionInputDTO input)
        {
            var userType = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await _unitOfWork.UserRepository.GetById(userId);

            if (user.Type is UserTypeEnum.Subscriber)
                return new APIResponse<CreateSubscriptionWithCreditCardResponse>(false, 400, "Usuário ja é Assinante");


            var paymentInfos = new PaymentInfosModel(user.Id, input.PostalCode, input.Address, input.AdressNumber, input.PhoneNumber, input.CpfCnpj, "");

            await _unitOfWork.BeginTransaction();

            await _unitOfWork.PaymentInfosRepository.Register(paymentInfos);


            string asaasCustomerId = "";
            var createCustomerRequest = new CreateCustumerRequest
                (
                $"{user.FirstName} {user.LastName}",
                input.CpfCnpj,
                user.Email,
                input.PhoneNumber,
                input.Address,
                input.AdressNumber,
                null,
                null,
                input.PostalCode,
                user.Id.ToString(),
                false,
                null,
                null,
                null
                );


            if (string.IsNullOrEmpty(user.AsaasCustomerId))
            {
                var createCustomerResponse = await _asaasIntegration.CreateCustumer(createCustomerRequest);

                if (createCustomerResponse is null)
                    return new APIResponse<CreateSubscriptionWithCreditCardResponse>(false, 400, "Ocorreu um erro em CreateCustomer do Asaas");

                asaasCustomerId = createCustomerResponse.id;

                user.AsaasCustomerId = createCustomerResponse.id;

                _unitOfWork.UserRepository.Update(user);
            }
            else
            {
                bool isAsaasCustomerExists = await _asaasIntegration.GetCustomerById(user.AsaasCustomerId);

                if (isAsaasCustomerExists is false)
                {
                    var createCustomerResponse = await _asaasIntegration.CreateCustumer(createCustomerRequest);

                    if (createCustomerResponse is null)
                        return new APIResponse<CreateSubscriptionWithCreditCardResponse>(false, 400, "Ocorreu um erro em CreateCustomer do Asaas");

                    asaasCustomerId = createCustomerResponse.id;

                    user.AsaasCustomerId = createCustomerResponse.id;

                    _unitOfWork.UserRepository.Update(user);
                }

                asaasCustomerId = user.AsaasCustomerId;
            }

            // FUTURAMENTE Criar Token do cartão de credito
            var creditCard = new CreditCard(input.CreditCardHolderName, input.CreditCardNumber, input.CreditCardExpiryMonth, input.CreditCardExpiryYear, input.ccv);
            var creditCardHolderInfo = new CreditCardHolderInfo(input.CreditCardHolderName, user.Email, input.CpfCnpj, input.PostalCode, input.AdressNumber, input.PhoneNumber);


            var createSubscriptionRequest = new CreateSubscriptionWithCreditCardRequest
                (
                asaasCustomerId,
                5,
                creditCard,
                creditCardHolderInfo,
                null,
                "192.168.112.1"
                );

            await _unitOfWork.SaveChanges();

            var createSubscriptionResponse = await _asaasIntegration.CreateSubscription(createSubscriptionRequest);

            if(createSubscriptionResponse is null)
            {
                await _unitOfWork.RollBack();
                return new APIResponse<CreateSubscriptionWithCreditCardResponse>(false, 400, "Ocorreu um erro em CreateSubscriptionWithCreditCard do Asaas");
            }

            await _unitOfWork.Commit();
               
            return new APIResponse<CreateSubscriptionWithCreditCardResponse>(true, 200, "Assinatura criada com sucesso!", createSubscriptionResponse, null);
        }

        public Task CreateSubscriptionByPaymentInfoId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task GetSubscriptionById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
