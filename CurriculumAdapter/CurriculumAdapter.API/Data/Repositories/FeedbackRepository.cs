using CurriculumAdapter.API.Data.Context;
using CurriculumAdapter.API.Data.Repositories.Interfaces;
using CurriculumAdapter.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CurriculumAdapter.API.Data.Repositories
{
    public class FeedbackRepository (DatabaseContext dbContext) : IFeedbackRepository
    {
        private readonly DatabaseContext _dbContext = dbContext;

        public async Task<IEnumerable<FeedbackModel>> GetAllFeedbacks()
            => await _dbContext.Feedbacks.ToListAsync();

        public async Task RegisterFeedback(FeedbackModel model)
        {
            await _dbContext.Feedbacks.AddAsync(model);
            await _dbContext.SaveChangesAsync();
        }
            

    }
}
