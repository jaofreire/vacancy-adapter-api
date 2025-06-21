using System.Text.Json.Serialization;

namespace CurriculumAdapter.API.Data.Integrations.Asaas.Request
{
    public class CreditCard
    {
        public string holderName { get; set; }
        public string number { get; set; }
        public string expiryMonth { get; set; }
        public string expiryYear { get; set; }
        public string ccv { get; set; }

        public CreditCard(string holderName, string number, string expiryMonth, string expiryYear, string ccv)
        {
            this.holderName = holderName;
            this.number = number;
            this.expiryMonth = expiryMonth;
            this.expiryYear = expiryYear;
            this.ccv = ccv;
        }
    }

    public class CreditCardHolderInfo
    {
        public string name { get; set; }
        public string email { get; set; }
        public string cpfCnpj { get; set; }
        public string postalCode { get; set; }
        public string addressNumber { get; set; }
        public string phone {  get; set; }

        public CreditCardHolderInfo(string name, string email, string cpfCnpj, string postalCode, string addressNumber, string phone)
        {
            this.name = name;
            this.email = email;
            this.cpfCnpj = cpfCnpj;
            this.postalCode = postalCode;
            this.addressNumber = addressNumber;
            this.phone = phone;
        }
    }

    public class CreateSubscriptionWithCreditCardRequest
    {
        public string customer {  get; set; }
        public string billingType { get; set; } = "CREDIT_CARD";
        public double value { get; set; }

        // Vencimento da primeiro cobrança
        public string nextDueDate { get; set; } = DateTime.Now.Date.ToString("yyyy-MM-dd");
        public string cycle { get; set; } = "MONTHLY";
        public string externalReference { get; set; } = "ca-subscriber";

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CreditCard? creditCard { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CreditCardHolderInfo? creditCardHolderInfo { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? creditCardToken { get; set; }

        //Ip do usuario
        public string remoteIp { get; set; }

        public CreateSubscriptionWithCreditCardRequest(
            string customer,
            double value,
            CreditCard? creditCard,
            CreditCardHolderInfo? creditCardHolderInfo,
            string? creditCardToken,
            string remoteIp
            )
        {
            this.customer = customer;
            this.value = value;
            this.creditCard = creditCard;
            this.creditCardHolderInfo = creditCardHolderInfo;
            this.creditCardToken = creditCardToken;
            this.remoteIp = remoteIp;
        }
    }
}
