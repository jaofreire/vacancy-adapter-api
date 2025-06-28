namespace CurriculumAdapter.API.Data.Integrations.Asaas.Response
{
    public class CustomerData
    {
        public string? id { get; set; }
        public string? dateCreated { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? mobilePhone { get; set; }
        public string? address { get; set; }
        public string? addressNumber { get; set; }
        public string? complement { get; set; }
        public string? province { get; set; }
        public string? cityName { get; set; }
        public string? state { get; set; }
        public string? country { get; set; }
        public string? postalCode { get; set; }
        public string? cpfCnpj { get; set; }
        public string? personType { get; set; }
        public bool deleted { get; set; }
        public string? additionalEmails { get; set; }
        public string? externalReference { get; set; }
        public bool notificationDisabled { get; set; }
        public string? observations { get; set; }
        public bool foreignCustomer { get; set; }
    }

    public class GetAllCustomersResponse
    {
        public bool hasMore { get; set; }
        public int totalCount { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public List<CustomerData> data { get; set; } = [];
    }
}
