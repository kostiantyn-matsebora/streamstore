using System.Threading;
using System.Threading.Tasks;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Provisioning;


namespace StreamStore.NoSql.Cassandra.Provisioning
{
    internal class CassandraSchemaProvisioner : ISchemaProvisioner
    {
        private readonly DataContextFactory contextFactory;

        public CassandraSchemaProvisioner(DataContextFactory contextFactory)
        {
            this.contextFactory = contextFactory.ThrowIfNull(nameof(contextFactory));
        }

        public async Task ProvisionSchemaAsync(CancellationToken token)
        {
            using (var ctx = contextFactory.Create())
            {
                await ctx.CreateIfNotExistsAsync();
            }
        }
    }
}
