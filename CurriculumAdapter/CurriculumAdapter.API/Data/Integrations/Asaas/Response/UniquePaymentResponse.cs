namespace CurriculumAdapter.API.Data.Integrations.Asaas.Response
{
    public class UniquePaymentResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }

        public bool Active { get; set; }

        public string ChargeType { get; set; }

        public string Url { get; set; }

        public string BillingType { get; set; }

        public string? SubscriptionCycle { get; set; }

        public string Description { get; set; }

        public DateTime EndDate { get; set; }

        public bool Deleted { get; set; }

        public int ViewCount { get; set; }

        public int MaxInstallmentCount { get; set; }

        public int DueDateLimitDays { get; set; }

        public bool NotificationEnabled { get; set; }

        public bool? IsAddressRequired { get; set; }

        public string? ExternalReference { get; set; }
    }
}
