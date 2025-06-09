using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace CurriculumAdapter.API.Data.Context
{
    public class QdrantContext
    {
        private readonly QdrantClient _client;
        public QdrantContext(string host)
        {
            _client = new QdrantClient(host);
        }

        public async Task<IReadOnlyList<ScoredPoint>> SearchBySimilarVectors(string collectionName, ulong searchLimit, ReadOnlyMemory<float> vectors)
        {
            var points = await _client.SearchAsync(
                collectionName,
                vectors,
                limit: searchLimit
                );

            return points;
        }
    }
}
