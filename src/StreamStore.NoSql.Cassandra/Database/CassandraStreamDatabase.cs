using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cassandra.Data.Linq;
using StreamStore.Database;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraStreamDatabase : StreamDatabaseBase
    {
        readonly ICassandraSessionFactory sessionFactory;
        readonly DataContextFactory contextFactory;

        public CassandraStreamDatabase(ICassandraSessionFactory sessionFactory, DataContextFactory contextFactory)
        {
            this.sessionFactory = sessionFactory.ThrowIfNull(nameof(sessionFactory));
            this.contextFactory = contextFactory.ThrowIfNull(nameof(contextFactory));
        }

        protected override Task<IStreamUnitOfWork> BeginAppendAsyncInternal(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
         return Task.FromResult<IStreamUnitOfWork>(
             new CassandraStreamUnitOfWork(streamId, expectedStreamVersion, null, sessionFactory, contextFactory));
        }

        protected override async Task DeleteAsyncInternal(Id streamId, CancellationToken token = default)
        {
            using (var session = sessionFactory.Open())
            {
                var ctx = contextFactory.Create(session);

                string id = (string)streamId;
                await ctx.Metadata.Where(er => er.StreamId == id).Delete().ExecuteAsync();
            }
        }

        protected override async Task<EventMetadataRecordCollection?> FindMetadataAsyncInternal(Id streamId, CancellationToken token = default)
        {
            using (var session = sessionFactory.Open())
            {
              var ctx  = contextFactory.Create(session);
             
              string id = (string)streamId;
              var metadata = await ctx.Metadata.Where(er => er.StreamId == id).ExecuteAsync();
              
               return new EventMetadataRecordCollection(metadata.ToArray().ToRecords());
            }
        }

        protected override async Task<int> GetActualRevision(Id streamId, CancellationToken token = default)
        {
            using (var session = sessionFactory.Open())
            {
                var ctx = contextFactory.Create(session);
                string id = (string)streamId;
                var revisions = (await ctx.Metadata.Where(er => er.StreamId == id).Select(er => er.Revision).ExecuteAsync()).ToArray();
                if (!revisions.Any())
                {
                    return Revision.Zero;
                }

                return revisions.Max();
            }
        }

        protected override async Task<EventRecord[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            using (var session = sessionFactory.Open())
            {
                var ctx = contextFactory.Create(session);
                string id = (string)streamId;
                int revision = (int)startFrom;
                var events = await ctx.Events.Where(er => er.StreamId == (string)id && er.Revision >= (int)revision).Take(count).ExecuteAsync();

                return events.ToArray().ToRecords();
            }
        }
    }
}
