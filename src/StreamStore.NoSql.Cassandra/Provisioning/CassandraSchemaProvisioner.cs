using System.Threading;
using System.Threading.Tasks;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.Provisioning;


namespace StreamStore.NoSql.Cassandra.Provisioning
{
    internal class CassandraSchemaProvisioner : ISchemaProvisioner
    {
        readonly ICassandraMapperProvider mapperProvider;
        readonly CassandraCqlQueries queries;

        public CassandraSchemaProvisioner(ICassandraMapperProvider mapperProvider, CassandraStorageConfiguration config)
        {

            this.mapperProvider = mapperProvider.ThrowIfNull(nameof(mapperProvider));
            this.queries = new CassandraCqlQueries(config.ThrowIfNull(nameof(config)));
        }

        public async Task ProvisionSchemaAsync(CancellationToken token)
        {
            var mapper = mapperProvider.OpenMapper();
            await mapper.ExecuteAsync(queries.CreateEventsTable());
        }
    }
}
