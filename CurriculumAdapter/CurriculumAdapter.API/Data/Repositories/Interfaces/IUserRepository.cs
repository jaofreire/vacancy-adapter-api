using CurriculumAdapter.API.Models;
using System.Linq.Expressions;

namespace CurriculumAdapter.API.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task Register(UserModel model);
        Task<IEnumerable<UserModel>> GetAll();
        Task<UserModel> GetById(Guid id);
        Task<IEnumerable<UserModel>> Get(Expression<Func<UserModel, bool>> predicate);
        void Update(UserModel model);
        void Delete(UserModel model);
        Task Commit();
    }
}
