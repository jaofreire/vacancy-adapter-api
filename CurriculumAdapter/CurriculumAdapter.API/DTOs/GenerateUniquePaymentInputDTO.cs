namespace CurriculumAdapter.API.DTOs
{
    public record GenerateUniquePaymentInputDTO
        (
        string Address,
        string PostalCode,
        string AdressNumber,
        string PhoneNumber,
        string CpfCnpj
        );
    
}
