using CurriculumAdapter.API.Data.Context;
using CurriculumAdapter.API.Data.Repositories.Interfaces;
using CurriculumAdapter.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace CurriculumAdapter.API.Data.Repositories
{
    public class PaymentInfosRepository(DatabaseContext context) : IPaymentInfosRepository
    {
        private readonly DatabaseContext _context = context;

        public async Task Register(PaymentInfosModel model)
            => await _context.PaymentInfos.AddAsync(model);
        

        public async Task<IEnumerable<PaymentInfosModel>> GetAll()
            => await _context.PaymentInfos.ToListAsync();


        public async Task<PaymentInfosModel> GetById(Guid id)
            => await _context.PaymentInfos.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<PaymentInfosModel>> Get(Expression<Func<PaymentInfosModel, bool>> predicate)
            => await _context.PaymentInfos.Where(predicate).ToListAsync();


        public void Update(PaymentInfosModel model)
            => _context.PaymentInfos.Update(model);


        public void Delete(PaymentInfosModel model)
            => _context.PaymentInfos.Remove(model);


        public async Task Commit()
            => await _context.SaveChangesAsync();

        
    }
}
