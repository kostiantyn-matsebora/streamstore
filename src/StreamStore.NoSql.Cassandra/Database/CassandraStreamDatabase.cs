using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Database;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraStreamDatabase : StreamDatabaseBase
    {
        readonly CassandraStreamRepositoryFactory repositoryFactory;

        public CassandraStreamDatabase(CassandraStreamRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory.ThrowIfNull(nameof(repositoryFactory));
        }

        protected override Task<IStreamUnitOfWork> BeginAppendAsyncInternal(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            return Task.FromResult<IStreamUnitOfWork>(
                new CassandraStreamUnitOfWork(streamId, expectedStreamVersion, null, repositoryFactory));
        }

        protected override async Task DeleteAsyncInternal(Id streamId, CancellationToken token = default)
        {
            using (var repo = repositoryFactory.Create())
            {
                await repo.DeleteStream(streamId);
            }
        }

        protected override async Task<EventMetadataRecordCollection?> FindMetadataAsyncInternal(Id streamId, CancellationToken token = default)
        {
            using (var repo = repositoryFactory.Create())
            {
                var metadata = (await repo.FindMetadata(streamId)).ToArray();

                if (!metadata.Any())
                {
                    return null;
                }

                return new EventMetadataRecordCollection(metadata.ToRecords());
            }
        }

        protected override async Task<int> GetActualRevision(Id streamId, CancellationToken token = default)
        {
            using (var repo = repositoryFactory.Create())
            {
                return await repo.GetStreamActualRevision(streamId);
            }
        }

        protected override async Task<EventRecord[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            using (var repo = repositoryFactory.Create())
            {
                var events = await repo.GetEvents(streamId, startFrom, count);
                return events.ToArray().ToRecords();
            }
        }
    }
}
