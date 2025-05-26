using CurriculumAdapter.API.Response;

namespace CurriculumAdapter.API.Services.Interface
{
    public interface IAdvisorService
    {
        Task<APIResponse<string>> SendPromptToAdvisorAssistant(string curriculumData);
    }
}
