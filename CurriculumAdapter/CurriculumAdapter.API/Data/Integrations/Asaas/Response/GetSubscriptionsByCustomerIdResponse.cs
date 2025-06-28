using CurriculumAdapter.API.DTOs;
using System.Text.Json.Serialization;

namespace CurriculumAdapter.API.Data.Integrations.Asaas.Response
{
    public class SubscriptionData
    {
        public string id { get; set; } = string.Empty;

        public DateTime dateCreated { get; set; }

        public string customer { get; set; } = string.Empty;

        public string paymentLink { get; set; } = string.Empty;

        public string billingType { get; set; } = string.Empty;

        public string cycle { get; set; } = string.Empty;

        public decimal value { get; set; }

        public DateTime nextDueDate { get; set; }

        public DateTime? endDate { get; set; }

        public string description { get; set; } = string.Empty;

        public string status { get; set; } = string.Empty;

        public bool deleted { get; set; }

        public int maxPayments { get; set; }

        public string externalReference { get; set; } = string.Empty;

        public string checkoutSession { get; set; } = string.Empty;
    }

    public class GetSubscriptionsByCustomerIdResponse
    {
        public bool hasMore {  get; set; }
        public int totalCount { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public List<SubscriptionData> data { get; set; } = [];

    }
}
