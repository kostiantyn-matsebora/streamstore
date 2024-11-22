using System.Threading;
using System.Threading.Tasks;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Provisioning;


namespace StreamStore.NoSql.Cassandra.Provisioning
{
    internal class CassandraSchemaProvisioner : ISchemaProvisioner
    {
        readonly ICassandraStreamRepositoryFactory contextFactory;

        public CassandraSchemaProvisioner(ICassandraStreamRepositoryFactory contextFactory)
        {
            this.contextFactory = contextFactory.ThrowIfNull(nameof(contextFactory));
        }

        public async Task ProvisionSchemaAsync(CancellationToken token)
        {
            using (var ctx = contextFactory.Create())
            {
                await ctx.CreateSchemaIfNotExistsAsync();
            }
        }
    }
}
