using CurriculumAdapter.API.Models;

namespace CurriculumAdapter.API.Data.Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        Task RegisterFeedback(FeedbackModel model);
        Task<IEnumerable<FeedbackModel>> GetAllFeedbacks();
    }
}
