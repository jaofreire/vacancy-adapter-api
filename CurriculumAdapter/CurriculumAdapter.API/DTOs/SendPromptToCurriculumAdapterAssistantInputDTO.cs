namespace CurriculumAdapter.API.DTOs
{
    public record SendPromptToCurriculumAdapterAssistantInputDTO
        (
        IFormFile File,
        string Description,
        string UserSkills = ""
        );
    
}
