using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using StreamStore.Exceptions;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraStreamUnitOfWork : StreamUnitOfWorkBase
    {
        readonly CassandraStreamRepositoryFactory contextFactory;

        public CassandraStreamUnitOfWork(
            Id streamId, 
            Revision expectedRevision, 
            EventRecordCollection? events, 
            CassandraStreamRepositoryFactory contextFactory)
            : base(streamId, expectedRevision, events)
        {
            this.contextFactory = contextFactory.ThrowIfNull(nameof(contextFactory));
        }

        protected override async Task SaveChangesAsync(EventRecordCollection uncommited, CancellationToken token)
        {
            using (var ctx = contextFactory.Create())
            {
              var result = await ctx.AppendToStream(streamId, uncommited.ToArray());
              await ValidateResult(ctx, result);
            }
        }

        private async Task ValidateResult(CassandraStreamRepository ctx, AppliedInfo<EventEntity> appliedInfo)
        {
            if (appliedInfo.Applied)
            {
                return;
            }
            var actualRevision = await ctx.GetStreamActualRevision(streamId);
            if (actualRevision != expectedRevision)
            {
                throw new OptimisticConcurrencyException(expectedRevision, actualRevision, streamId);
            }
        }
    }
}
