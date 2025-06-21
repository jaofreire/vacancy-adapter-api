using CurriculumAdapter.API.Models;
using System.Linq.Expressions;

namespace CurriculumAdapter.API.Data.Repositories.Interfaces
{
    public interface IPaymentInfosRepository
    {
        Task Register(PaymentInfosModel model);
        Task<IEnumerable<PaymentInfosModel>> GetAll();
        Task<PaymentInfosModel> GetById(Guid id);
        Task<IEnumerable<PaymentInfosModel>> Get(Expression<Func<PaymentInfosModel, bool>> predicate);
        void Update(PaymentInfosModel model);
        void Delete(PaymentInfosModel model);
        Task Commit();
    }
}
