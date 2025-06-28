namespace CurriculumAdapter.API.Data.Integrations.Asaas.Response
{
    public class PaymentData
    {
        public string id { get; set; } = string.Empty;

        public DateTime dateCreated { get; set; }

        public string subscription { get; set; } = string.Empty;

        public string installment { get; set; } = string.Empty;

        public string checkoutSession { get; set; } = string.Empty;

        public string paymentLink {  get; set; } = string.Empty;

        public decimal value { get; set; }

        public string description { get; set; } = string.Empty;

        public string billingType { get; set; } = string.Empty;

        public string cycle { get; set; } = string.Empty;

        public DateTime nextDueDate { get; set; }

        public DateTime? endDate { get; set; }

        public CreditCardResponse? creditCard { get; set; }

        public bool canBePaidAfterDueDate { get; set; }

        public string pixTransaction { get; set; } = string.Empty;

        public string pixQrCodeId { get; set; } = string.Empty;

        public string status { get; set; } = string.Empty;

        public string invoiceUrl {  get; set; } = string.Empty;

        public string transactionReceipUrl {  get; set; } = string.Empty;

        public bool deleted { get; set; }

        public int maxPayments { get; set; }

        public string externalReference { get; set; } = string.Empty;

        
    }
    
    public class GetPaymentsBySubscriptionIdResponse
    {
        public bool hasMore { get; set; }
        public int totalCount { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public List<PaymentData> data { get; set; } = [];
    }
}
