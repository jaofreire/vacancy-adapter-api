namespace CurriculumAdapter.API.DTOs
{
    public record SendPromptToCurriculumAdapterAssistantInputDTO
        (
        string RecaptchaToken,
        IFormFile File,
        string Description,
        string UserSkills = ""
        );
    
}
