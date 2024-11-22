using System.Threading;
using System.Threading.Tasks;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Provisioning;


namespace StreamStore.NoSql.Cassandra.Provisioning
{
    internal class CassandraSchemaProvisioner : ISchemaProvisioner
    {
        readonly ICassandraStreamRepositoryFactory repoFactory;

        public CassandraSchemaProvisioner(ICassandraStreamRepositoryFactory contextFactory)
        {
            this.repoFactory = contextFactory.ThrowIfNull(nameof(contextFactory));
        }

        public async Task ProvisionSchemaAsync(CancellationToken token)
        {
            using (var repo = repoFactory.Create())
            {
                await repo.CreateSchemaIfNotExistsAsync();
            }
        }
    }
}
