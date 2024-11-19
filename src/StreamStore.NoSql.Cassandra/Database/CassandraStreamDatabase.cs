using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Database;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraStreamDatabase : StreamDatabaseBase
    {
        readonly DataContextFactory contextFactory;

        public CassandraStreamDatabase(DataContextFactory contextFactory)
        {
            this.contextFactory = contextFactory.ThrowIfNull(nameof(contextFactory));

        }

        protected override Task<IStreamUnitOfWork> BeginAppendAsyncInternal(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            return Task.FromResult<IStreamUnitOfWork>(
                new CassandraStreamUnitOfWork(streamId, expectedStreamVersion, null, contextFactory));
        }

        protected override async Task DeleteAsyncInternal(Id streamId, CancellationToken token = default)
        {
            using (var ctx = contextFactory.Create())
            {
                await ctx.DeleteStream(streamId).ExecuteAsync();
            }
        }

        protected override async Task<EventMetadataRecordCollection?> FindMetadataAsyncInternal(Id streamId, CancellationToken token = default)
        {
            using (var ctx = contextFactory.Create())
            {
                var metadata = (await ctx.FindMetadata(streamId).ExecuteAsync()).ToArray();

                if (!metadata.Any())
                {
                    return null;
                }

                return new EventMetadataRecordCollection(metadata.ToArray().ToRecords());
            }
        }

        protected override async Task<int> GetActualRevision(Id streamId, CancellationToken token = default)
        {
            using (var ctx = contextFactory.Create())
            {
                var revisions =  await ctx.GetStreamRevisions(streamId).ExecuteAsync();
                if (!revisions.Any())
                {
                    return Revision.Zero;
                }

                return revisions.Max();
            }
        }

        protected override async Task<EventRecord[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            using (var ctx = contextFactory.Create())
            {
                var events = await ctx.GetEvents(streamId, startFrom, count).ExecuteAsync();
                return events.ToArray().ToRecords();
            }
        }
    }
}
