using CurriculumAdapter.API.Models.Logs;
using System.Linq.Expressions;

namespace CurriculumAdapter.API.Data.Repositories.Interfaces
{
    public interface IFeatureUsageLogRepository
    {
        Task Register(FeatureUsageLogModel model);
        Task<IEnumerable<FeatureUsageLogModel>> GetAll();
        Task<FeatureUsageLogModel> GetById(Guid id);
        Task<IEnumerable<FeatureUsageLogModel>> Get(Expression<Func<FeatureUsageLogModel, bool>> predicate);
        Task Commit();
    }
}
