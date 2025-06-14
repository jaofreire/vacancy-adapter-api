using CurriculumAdapter.API.Data.Context;
using CurriculumAdapter.API.Data.Repositories.Interfaces;
using CurriculumAdapter.API.Models.Logs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CurriculumAdapter.API.Data.Repositories
{
    public class FeatureUsageLogRepository(DatabaseContext context) : IFeatureUsageLogRepository
    {
        private readonly DatabaseContext _context = context;

        public async Task Register(FeatureUsageLogModel model)
            => await _context.FeatureUsageLogs.AddAsync(model);

        public async Task<IEnumerable<FeatureUsageLogModel>> GetAll()
            => await _context.FeatureUsageLogs.ToListAsync();

        public async Task<FeatureUsageLogModel> GetById(Guid id)
            => await _context.FeatureUsageLogs.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<FeatureUsageLogModel>> Get(Expression<Func<FeatureUsageLogModel, bool>> predicate)
            => await _context.FeatureUsageLogs.Where(predicate).ToListAsync();

        public Task Commit()
            => _context.SaveChangesAsync();
    }
}
