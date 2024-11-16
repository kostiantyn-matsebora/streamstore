using System.Threading;
using System.Threading.Tasks;
using Cassandra;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraStreamUnitOfWork : StreamUnitOfWorkBase
    {
        readonly ICassandraSessionFactory sessionFactory;
        readonly DataContextFactory contextFactory;

        public CassandraStreamUnitOfWork(Id streamId, Revision expectedRevision, EventRecordCollection? events, ICassandraSessionFactory sessionFactory, DataContextFactory contextFactory)
            : base(streamId, expectedRevision, events)
        {
            this.sessionFactory = sessionFactory.ThrowIfNull(nameof(sessionFactory));
            this.contextFactory = contextFactory.ThrowIfNull(nameof(contextFactory));
        }

        protected override async Task SaveChangesAsync(EventRecordCollection uncommited, CancellationToken token)
        {
            using (var session = sessionFactory.CreateSession())
            {
                var statement = new BatchStatement();
                statement.SetConsistencyLevel(ConsistencyLevel.Quorum);

                var ctx = contextFactory.Create(session);
                foreach (var record in uncommited)
                {

                    statement.Add(ctx.Events.Insert(record.ToEntity(streamId)));
                    statement.Add(ctx.RevisionPerStream.Insert(new RevisionPerStreamEntity { Revision = record.Revision, StreamId = streamId }));
                    statement.Add(ctx.EventPerStream.Insert(new EventPerStreamEntity { StreamId = streamId, Id = record.Id }));
                }
                await session.ExecuteAsync(statement);
            }
        }
    }
}
