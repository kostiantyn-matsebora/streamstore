using System.Threading;
using System.Threading.Tasks;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Provisioning;


namespace StreamStore.NoSql.Cassandra.Provisioning
{
    internal class CassandraSchemaProvisioner : ISchemaProvisioner
    {
        readonly ICassandraMapperProvider mapperProvider;
        private readonly CassandraStorageConfiguration config;

        public CassandraSchemaProvisioner(ICassandraMapperProvider mapperProvider, CassandraStorageConfiguration config)
        {

            this.mapperProvider = mapperProvider.ThrowIfNull(nameof(mapperProvider));
            this.config = config.ThrowIfNull(nameof(config));
        }

        public async Task ProvisionSchemaAsync(CancellationToken token)
        {
            using (var mapper = mapperProvider.OpenMapper())
            {
                await mapper.ExecuteAsync(@$"CREATE TABLE IF NOT EXISTS {config.EventsTableName}
                        (id text,
                        stream_id text,
                        revision int,
                        timestamp timestamp,
                        data blob,
                        PRIMARY KEY(stream_id, revision)
                        );");
            }
        }
    }
}
