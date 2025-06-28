namespace CurriculumAdapter.API.DTOs
{
    public record CreditCardInputDTO
        (
        string HolderName,
        string Number,
        string ExpiryMonth,
        string ExpiryYear,
        string Ccv
        );
    
}
