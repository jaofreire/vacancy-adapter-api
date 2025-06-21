namespace CurriculumAdapter.API.Data.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IFeatureUsageLogRepository FeatureUsageLogRepository { get; }
        IFeedbackRepository FeedbackRepository { get; }
        IJobsCollectionRepository JobsCollectionRepository { get; }
        IPaymentInfosRepository PaymentInfosRepository { get; }
        IUserRepository UserRepository { get; }
        Task BeginTransaction();
        Task Commit();
        Task RollBack();
    }
}
