namespace CurriculumAdapter.API.DTOs
{
    public record CreateSubscriptionInputDTO
        (
        string Address,
        string PostalCode,
        string AdressNumber,
        string PhoneNumber,
        string CpfCnpj,
        string CreditCardHolderName,
        string CreditCardNumber,
        string CreditCardExpiryMonth, 
        string CreditCardExpiryYear,
        string ccv
        );
    
}
