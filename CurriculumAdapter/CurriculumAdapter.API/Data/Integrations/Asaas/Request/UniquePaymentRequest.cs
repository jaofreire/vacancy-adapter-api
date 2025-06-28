namespace CurriculumAdapter.API.Data.Integrations.Asaas.Request
{
    public class UniquePaymentRequest
    {
        public string customer { get; set; }
        public string description { get; set; } = "Pagamento único disponibilizando acesso ilimitado ao CurriculumAdapter por 1 mês";
        public string dueDate { get; set; } = DateTime.Now.AddDays(1).Date.ToString("yyyy-MM-dd");
        public double value { get; set; } = 5;
        public string billingType { get; set; } = "UNDEFINED";
        public string externalReference { get; set; } = "unique-payment";

        public UniquePaymentRequest(string customer)
        {
            this.customer = customer;
        }
    }
}
