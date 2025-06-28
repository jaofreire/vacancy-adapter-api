namespace CurriculumAdapter.API.Data.Integrations.Asaas.Response
{
    public class CreditCardResponse
    {
        public string creditCardNumber { get; set; }
        public string creditCardBrand { get; set; }
        public string creditCardToken { get; set; }
    }

    public class CreateSubscriptionWithCreditCardResponse
    {
        public string? Object {  get; set; }
        public string? id { get; set; }
        public string? dateCreated { get; set; }
        public string? customer {  get; set; }
        public string? paymentLink { get; set; }
        public string? billingType { get; set; }
        public string? cycle {  get; set; }
        public double value { get; set; }
        public DateTime nextDueDate { get; set; }
        public bool deleted { get; set; }
        public CreditCardResponse creditCard { get; set; }
    }
}
