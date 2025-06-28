using CurriculumAdapter.API.Data.Repositories.Interfaces;
using CurriculumAdapter.API.Models;
using CurriculumAdapter.API.Response;
using CurriculumAdapter.API.Services.Interface;

namespace CurriculumAdapter.API.Services
{
    public class FeedbackService(IUnitOfWork unitOfWork) : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<APIResponse<FeedbackModel>> GetAllFeedback()
        {
            var feedbacks = await _unitOfWork.FeedbackRepository.GetAllFeedbacks();

            return new APIResponse<FeedbackModel>(true, 200, "Feedbacks recuperados!", null, feedbacks);
        }

        public async Task<APIResponse<FeedbackModel>> RegisterFeedback(FeedbackModel model)
        {
            await _unitOfWork.BeginTransaction();

            await _unitOfWork.FeedbackRepository.RegisterFeedback(model);
            await _unitOfWork.Commit();

            return new APIResponse<FeedbackModel>(true, 200, "Feedback Registrado!");
        }
    }
}
