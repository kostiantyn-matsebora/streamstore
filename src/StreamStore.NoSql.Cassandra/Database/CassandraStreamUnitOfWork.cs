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
        private readonly CassandraStatementConfigurator configurator;

        public CassandraStreamUnitOfWork(
            Id streamId, 
            Revision expectedRevision, 
            EventRecordCollection? events, 
            ICassandraSessionFactory sessionFactory, 
            DataContextFactory contextFactory,
            CassandraStatementConfigurator configurator)
            : base(streamId, expectedRevision, events)
        {
            this.sessionFactory = sessionFactory.ThrowIfNull(nameof(sessionFactory));
            this.contextFactory = contextFactory.ThrowIfNull(nameof(contextFactory));
            this.configurator = configurator.ThrowIfNull(nameof(configurator));
        }

        protected override async Task SaveChangesAsync(EventRecordCollection uncommited, CancellationToken token)
        {
            using (var session = sessionFactory.Open())
            {
                var ctx = contextFactory.Create(session);
                foreach (var record in uncommited)
                {
                    var statement =
                        configurator.ConfigureInsert(
                            ctx.Events.Insert(record.ToEntity(streamId)).IfNotExists());
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
                        var actualRevision = await CassandraStreamActualRevisionResolver.Resolve(configurator, ctx, streamId);
                        throw new OptimisticConcurrencyException(expectedRevision, expectedRevision, streamId);
                    }
                }
               
            }
        }
    }
}
