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

        public async Task<APIResponse<CreateSubscriptionWithCreditCardResponse>> CreateSubscriptionByPaymentInfoId(Guid id, CreditCardInputDTO input)
        {
            var userType = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await _unitOfWork.UserRepository.GetById(userId);

            if (user.Type is UserTypeEnum.Subscriber)
                return new APIResponse<CreateSubscriptionWithCreditCardResponse>(false, 400, "Usuário ja é Assinante");

            var paymentInfo = await _unitOfWork.PaymentInfosRepository.GetById(id);

            if (paymentInfo is null)
                return new APIResponse<CreateSubscriptionWithCreditCardResponse>(false, 404, "Dados de pagamento não encontrados");

            var creditCard = new CreditCard(input.HolderName, input.Number, input.ExpiryMonth, input.ExpiryYear, input.Ccv);
            var creditCardHolderInfo = new CreditCardHolderInfo($"{user.FirstName} + {user.LastName}", user.Email, paymentInfo.CpfCnpj, paymentInfo.PostalCode, paymentInfo.AdressNumber, paymentInfo.PhoneNumber);

            var subscription = await _asaasIntegration.CreateSubscription(new CreateSubscriptionWithCreditCardRequest(user.AsaasCustomerId, 5, creditCard, creditCardHolderInfo, "", "192.168.112.1"));

            if(subscription is null)
                return new APIResponse<CreateSubscriptionWithCreditCardResponse>(false, 400, "Ocorreu um erro ao Criar Assinatura");

            return new APIResponse<CreateSubscriptionWithCreditCardResponse>(true, 200, "Assinatura criada com sucesso", subscription, null);
        }

        public async Task<APIResponse<UniquePaymentResponse>> GenerateUniquePayment(GenerateUniquePaymentInputDTO input)
        {
            var userType = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await _unitOfWork.UserRepository.GetById(userId);

            if (user.Type is UserTypeEnum.Subscriber)
                return new APIResponse<UniquePaymentResponse>(false, 400, "Usuário ja é Assinante");

            await _unitOfWork.BeginTransaction();

            var paymentInfos = new PaymentInfosModel(user.Id, input.PostalCode, input.Address, input.AdressNumber, input.PhoneNumber, input.CpfCnpj, "");
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
                    return new APIResponse<UniquePaymentResponse>(false, 400, "Ocorreu um erro em CreateCustomer do Asaas");

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
                        return new APIResponse<UniquePaymentResponse>(false, 400, "Ocorreu um erro em CreateCustomer do Asaas");

                    asaasCustomerId = createCustomerResponse.id;

                    user.AsaasCustomerId = createCustomerResponse.id;

                    _unitOfWork.UserRepository.Update(user);
                }

                asaasCustomerId = user.AsaasCustomerId;
            }

            await _unitOfWork.SaveChanges();

            var uniquePayment = await _asaasIntegration.UniquePayment(new UniquePaymentRequest(asaasCustomerId));

            if (uniquePayment is null)
            {
                await _unitOfWork.RollBack();
                return new APIResponse<UniquePaymentResponse>(false, 400, "Ocorreu um erro ao gerar Cobrança única");
            }
               
            await _unitOfWork.Commit();

            return new APIResponse<UniquePaymentResponse>(true, 200, "Cobrança única gerada com sucesso", uniquePayment, null);
        }

        public async Task<APIResponse<UniquePaymentResponse>> GenerateUniquePaymentWithPaymentInfoId(Guid paymentInfoId)
        {
            var userType = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await _unitOfWork.UserRepository.GetById(userId);

            if (user.Type is UserTypeEnum.Subscriber)
                return new APIResponse<UniquePaymentResponse>(false, 400, "Usuário ja é Assinante");

            var paymentInfo = await _unitOfWork.PaymentInfosRepository.GetById(paymentInfoId);

            if (paymentInfo is null)
                return new APIResponse<UniquePaymentResponse>(false, 404, "Dados de pagamento não encontrados");

            var uniquePayment = await _asaasIntegration.UniquePayment(new UniquePaymentRequest(user.AsaasCustomerId));

            if(uniquePayment is null)
                return new APIResponse<UniquePaymentResponse>(false, 400, "Ocorreu um erro ao gerar Pagamento Único");

            return new APIResponse<UniquePaymentResponse>(true, 200, "Pagamento Único gerado com sucesso", uniquePayment, null);
        }

        public Task GetSubscriptionById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<APIResponse<GetSubscriptionsByCustomerIdResponse>> GetSubscriptionsByCustomerId(string customerId)
        {
            var subscriptions = await _asaasIntegration.GetSubscriptionsByCustomerId(customerId);

            if (subscriptions is null)
                return new APIResponse<GetSubscriptionsByCustomerIdResponse>(false, 400, "Ocorreu um erro ao buscar Assinaturas de um cliente");

            return new APIResponse<GetSubscriptionsByCustomerIdResponse>(true, 200, "Assinaturas de um cliente listadas com sucesso", subscriptions, null);
        }

        public async Task<APIResponse<GetPaymentsBySubscriptionIdResponse>> GetPaymentsBySubscriptionId(string subscriptionId)
        {
            var payments = await _asaasIntegration.GetPaymentsBySubscriptionId(subscriptionId);

            if (payments is null)
                return new APIResponse<GetPaymentsBySubscriptionIdResponse>(false, 400, "Ocorreu um erro ao buscar Cobranças de uma assinatura");

            return new APIResponse<GetPaymentsBySubscriptionIdResponse>(true, 200, "Cobranças de uma assinatura listadas com sucesso", payments, null);
        }

        public async Task<APIResponse<GetUniquePaymentsByCustomerIdResponse>> GetUniquePaymentsByCustomerId(string customerId)
        {
            var payments = await _asaasIntegration.GetUniquePaymentsByCustomerId(customerId);

            if (payments is null)
                return new APIResponse<GetUniquePaymentsByCustomerIdResponse>(false, 400, "Ocorreu um erro ao buscar Cobranças de uma assinatura");

            return new APIResponse<GetUniquePaymentsByCustomerIdResponse>(true, 200, "Cobranças de uma assinatura listadas com sucesso", payments, null);
        }

        public async Task<APIResponse<bool>> RemoveSubscription(string subscriptionId)
        {
            var removeResponse = await _asaasIntegration.RemoveSubscription(subscriptionId);

            if (!removeResponse)
                return new APIResponse<bool>(false, 400, "Ocorreu um erro ao remover Assinatura");

            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await _unitOfWork.UserRepository.GetById(userId);

            if(user is null)
                return new APIResponse<bool>(false, 404, "Usuário não encontrado");

            user.Type = UserTypeEnum.Default;

            await _unitOfWork.BeginTransaction();

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.Commit();

            return new APIResponse<bool>(true, 200, "Assinatura removida com sucesso");
        }
    }
}
