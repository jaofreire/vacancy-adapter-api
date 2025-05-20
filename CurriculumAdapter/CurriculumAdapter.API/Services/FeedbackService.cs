using CurriculumAdapter.API.Data.Repositories.Interfaces;
using CurriculumAdapter.API.Models;
using CurriculumAdapter.API.Response;
using CurriculumAdapter.API.Services.Interface;

namespace CurriculumAdapter.API.Services
{
    public class FeedbackService(IFeedbackRepository repository) : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository = repository;

        public async Task<APIResponse<FeedbackModel>> GetAllFeedback()
        {
            var feedbacks = await _feedbackRepository.GetAllFeedbacks();

            return new APIResponse<FeedbackModel>(true, 200, "Feedbacks recuperados!", null, feedbacks);
        }

        public async Task<APIResponse<FeedbackModel>> RegisterFeedback(FeedbackModel model)
        {
            await _feedbackRepository.RegisterFeedback(model);

            return new APIResponse<FeedbackModel>(true, 200, "Feedback Registrado!");
        }
    }
}
