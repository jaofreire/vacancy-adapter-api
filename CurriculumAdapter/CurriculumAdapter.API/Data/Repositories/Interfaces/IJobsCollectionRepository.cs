using Qdrant.Client.Grpc;

namespace CurriculumAdapter.API.Data.Repositories.Interfaces
{
    public interface IJobsCollectionRepository
    {
        Task<IReadOnlyList<ScoredPoint>> SearchJobsBySimilarVectors(ReadOnlyMemory<float> vectors);
    }
}
