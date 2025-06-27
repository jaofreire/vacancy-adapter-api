namespace CurriculumAdapter.API.Data.Integrations.Asaas.Response
{
    public class GetUniquePaymentsByCustomerIdResponse
    {
        public bool hasMore { get; set; }
        public int totalCount { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public List<PaymentData> data { get; set; } = [];
    }
}
