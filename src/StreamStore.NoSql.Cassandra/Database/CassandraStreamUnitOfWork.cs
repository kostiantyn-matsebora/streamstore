using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cassandra.Mapping;
using StreamStore.Exceptions;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraStreamUnitOfWork : StreamUnitOfWorkBase
    {
        readonly CassandraStreamRepositoryFactory repositoryFactory;

        public CassandraStreamUnitOfWork(
            Id streamId, 
            Revision expectedRevision, 
            EventRecordCollection? events, 
            CassandraStreamRepositoryFactory repositoryFactory)
            : base(streamId, expectedRevision, events)
        {
            this.repositoryFactory = repositoryFactory.ThrowIfNull(nameof(repositoryFactory));
        }

        protected override async Task SaveChangesAsync(EventRecordCollection uncommited, CancellationToken token)
        {
            using (var repo = repositoryFactory.Create())
            {
              var result = await repo.AppendToStream(streamId, uncommited.ToArray());
              await ValidateResult(repo, result);
            }
        }

        async Task ValidateResult(CassandraStreamRepository repo, AppliedInfo<EventEntity> appliedInfo)
        {
            if (appliedInfo.Applied)
            {
                return;
            }
            var actualRevision = await repo.GetStreamActualRevision(streamId);
            if (actualRevision != expectedRevision)
            {
                throw new OptimisticConcurrencyException(expectedRevision, actualRevision, streamId);
            }
        }
    }
}
