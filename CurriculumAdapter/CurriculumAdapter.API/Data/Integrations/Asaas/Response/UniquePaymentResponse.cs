namespace CurriculumAdapter.API.Data.Integrations.Asaas.Response
{
    public class UniquePaymentResponse
    {
        public string id { get; set; }
        public DateTime dateCreated { get; set; }
        public string customer { get; set; }
        public string subscription { get; set; }
        public string installment { get; set; }
        public string checkoutSession { get; set; }
        public string invoiceUrl { get; set; }
        public decimal value { get; set; }
        public decimal netValue { get; set; }
        public decimal? originalValue { get; set; }
        public decimal? interestValue { get; set; }
        public string description { get; set; }
        public string billingType { get; set; }
    }
}
