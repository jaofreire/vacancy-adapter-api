using CurriculumAdapter.API.Models.Enums;

namespace CurriculumAdapter.API.DTOs
{
    public record RegisterUserInputDTO
        (
        string FirstName,
        string LastName,
        string Email,
        string Password
        );
    
}
