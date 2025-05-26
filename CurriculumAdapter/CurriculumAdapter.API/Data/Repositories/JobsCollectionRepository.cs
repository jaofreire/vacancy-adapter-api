using CurriculumAdapter.API.Data.Context;
using CurriculumAdapter.API.Data.Repositories.Interfaces;
using Qdrant.Client.Grpc;

namespace CurriculumAdapter.API.Data.Repositories
{
    public class JobsCollectionRepository(QdrantContext context) : IJobsCollectionRepository
    {
        private readonly QdrantContext _context = context;

        public async Task<IReadOnlyList<ScoredPoint>> SearchJobsBySimilarVectors(ReadOnlyMemory<float> vectors)
        {
            var jobs = await _context.SearchBySimilarVectors("vagas", 20, vectors);

            return jobs;
        }
    }
}
