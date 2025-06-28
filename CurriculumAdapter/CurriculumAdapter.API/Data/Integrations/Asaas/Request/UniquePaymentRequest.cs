namespace CurriculumAdapter.API.Data.Integrations.Asaas.Request
{
    public class UniquePaymentRequest
    {
        public string name { get; set; } = "Pagamento único Curriculum Adapter";
        public string description { get; set; } = "Pagamento único disponibilizando acesso ilimitado ao CurriculumAdapter por 1 mês";
        public string endDate { get; set; } = DateTime.Now.AddDays(1).Date.ToString();
        public double value { get; set; } = 5;
        public string billingType { get; set; } = "UNDEFINED";
        public string chargeType { get; set; } = "DETACHED";
        public int dueDateLimitDays { get; set; } = 1;
    }
}
