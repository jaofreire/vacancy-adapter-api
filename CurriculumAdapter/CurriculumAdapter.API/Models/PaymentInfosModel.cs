using System.Text.Json.Serialization;

namespace CurriculumAdapter.API.Models
{
    public class PaymentInfosModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string AdressNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string CpfCnpj { get; set; }
        public string CreditCardToken { get; set; }

        [JsonConstructor]
        public PaymentInfosModel()
        {
        }

        public PaymentInfosModel(
            Guid userId,
            string postalCode,
            string address,
            string adressNumber,
            string phoneNumber,
            string cpfCnpj,
            string creditCardToken
            )
        {
            Id = Guid.NewGuid();
            UserId = userId;
            PostalCode = postalCode;
            Address = address;
            AdressNumber = adressNumber;
            PhoneNumber = phoneNumber;
            CpfCnpj = cpfCnpj;
            CreditCardToken = creditCardToken;
        }
    }
}
