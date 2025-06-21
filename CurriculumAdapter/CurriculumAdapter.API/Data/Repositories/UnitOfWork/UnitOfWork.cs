using CurriculumAdapter.API.Data.Context;
using CurriculumAdapter.API.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CurriculumAdapter.API.Data.Repositories.UnitOfWork
{
    public class UnitOfWork(DatabaseContext context, QdrantContext qdrantContext) : IUnitOfWork
    {
        private readonly DatabaseContext _context = context;
        private readonly QdrantContext _qdrantContext = qdrantContext;
        private IDbContextTransaction? _transaction;

        private IFeatureUsageLogRepository? _featureUsageLogRepository;
        private IFeedbackRepository? _feedbackRepository;
        private IJobsCollectionRepository? _jobsCollectionRepository;
        private IPaymentInfosRepository? _paymentInfosRepository;
        private IUserRepository? _userRepository;

        public IFeatureUsageLogRepository FeatureUsageLogRepository => _featureUsageLogRepository ??= new FeatureUsageLogRepository(_context);

        public IFeedbackRepository FeedbackRepository => _feedbackRepository ??= new FeedbackRepository(_context);

        public IJobsCollectionRepository JobsCollectionRepository => _jobsCollectionRepository ??= new JobsCollectionRepository(_qdrantContext);

        public IPaymentInfosRepository PaymentInfosRepository => _paymentInfosRepository ??= new PaymentInfosRepository(_context);

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

        public async Task BeginTransaction()
        {
            if (_transaction == null)
                _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            try
            {
                await _context.SaveChangesAsync();

                if(_transaction != null)
                {
                    await _transaction.CommitAsync();
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }

            }
            catch (Exception ex)
            {
                await RollBack();
                Console.WriteLine(ex);
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        public async Task RollBack()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
