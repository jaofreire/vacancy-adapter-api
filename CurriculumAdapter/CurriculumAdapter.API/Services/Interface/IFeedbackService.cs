using CurriculumAdapter.API.Models;
using CurriculumAdapter.API.Response;

namespace CurriculumAdapter.API.Services.Interface
{
    public interface IFeedbackService
    {
        Task<APIResponse<FeedbackModel>> RegisterFeedback(FeedbackModel model);
        Task<APIResponse<FeedbackModel>> GetAllFeedback();
    }
}
