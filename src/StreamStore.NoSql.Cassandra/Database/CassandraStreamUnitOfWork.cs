using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Data.Linq;
using StreamStore.Exceptions;
using StreamStore.NoSql.Cassandra.API;
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
            using (var session = sessionFactory.Open())
            {
                var batch = new BatchStatement();
                batch.SetConsistencyLevel(ConsistencyLevel.All);
                batch.SetSerialConsistencyLevel(ConsistencyLevel.Serial);
                batch.SetBatchType(BatchType.Logged);
                var ctx = contextFactory.Create(session);
                foreach (var record in uncommited)
                {
                    var statement =
                        ctx.Events.Insert(record.ToEntity(streamId))
                        .IfNotExists()
                        .SetConsistencyLevel(ConsistencyLevel.Quorum)
                        .SetSerialConsistencyLevel(ConsistencyLevel.Serial);
                 
                    var result = await session.ExecuteAsync(statement);
                    await ValidateResult(ctx, result);
                }
            }
        }

        private async Task ValidateResult(DataContext ctx, RowSet result)
        {
            if (result.Columns.Any(c => c.Name == "[applied]"))
            {
                var row = result.FirstOrDefault();
                if (row !=  default)
                {
                    var applied = row.GetValue<bool>("[applied]");
                    if (!applied)
                    {
                        var actualRevision = await CassandraStreamActualRevisionResolver.Resolve(ctx, streamId);
                        throw new OptimisticConcurrencyException(expectedRevision, expectedRevision, streamId);
                    }
                }
               
            }
        }
    }
}
