using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cassandra.Data.Linq;
using StreamStore.Database;
using StreamStore.Exceptions;
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
            throw new NotImplementedException();
        }

        protected override async Task DeleteAsyncInternal(Id streamId, CancellationToken token = default)
        {
            using (var session = sessionFactory.CreateSession())
            {
                var ctx = contextFactory.Create(session);

                await ctx.Metadata.Where(er => er.StreamId == streamId).Delete().ExecuteAsync();
            }
        }

        protected override async Task<EventMetadataRecordCollection?> FindMetadataAsyncInternal(Id streamId, CancellationToken token = default)
        {
            using (var session = sessionFactory.CreateSession())
            {
              var ctx  = contextFactory.Create(session);
             
              var metadata = await ctx.Metadata.Where(er => er.StreamId == streamId).ExecuteAsync();
              
                return new EventMetadataRecordCollection(metadata.ToArray().ToRecords());
            }
        }

        protected override Task<int> GetActualRevision(Id streamId, CancellationToken token = default)
        {
            using (var session = sessionFactory.CreateSession())
            {
                var ctx = contextFactory.Create(session);

                var revision = ctx.Metadata.Where(er => er.StreamId == streamId).Max(e => e.Revision);

                if (revision == default(Revision))
                {
                    throw new StreamNotFoundException(streamId);
                }
                return Task.FromResult(revision);
            }
        }

        protected override async Task<EventRecord[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            using (var session = sessionFactory.CreateSession())
            {
                var ctx = contextFactory.Create(session);

                var events = await ctx.Events.Where(er => er.StreamId == streamId && er.Revision >= startFrom).Take(count).ExecuteAsync();

                return events.ToArray().ToRecords();
            }
        }
    }
}
