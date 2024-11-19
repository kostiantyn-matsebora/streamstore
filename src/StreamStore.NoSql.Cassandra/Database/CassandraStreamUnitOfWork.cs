using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Data.Linq;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraStreamUnitOfWork : StreamUnitOfWorkBase
    {
        readonly DataContextFactory contextFactory;

        public CassandraStreamUnitOfWork(
            Id streamId, 
            Revision expectedRevision, 
            EventRecordCollection? events, 
            DataContextFactory contextFactory)
            : base(streamId, expectedRevision, events)
        {
            this.contextFactory = contextFactory.ThrowIfNull(nameof(contextFactory));
        }

        protected override async Task SaveChangesAsync(EventRecordCollection uncommited, CancellationToken token)
        {
            using (var ctx = contextFactory.Create())
            {
                foreach (var record in uncommited)
                {
                    var result = await ctx.AppendToStream(streamId, record).ExecuteAsync();
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
                        var actualRevision = (await ctx.GetStreamRevisions(streamId).ExecuteAsync()).Max();
                    }
                }
               
            }
        }
    }
}
