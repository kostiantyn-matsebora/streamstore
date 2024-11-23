using System.Threading;
using System.Threading.Tasks;
using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Provisioning;


namespace StreamStore.NoSql.Cassandra.Provisioning
{
    internal class CassandraSchemaProvisioner : ISchemaProvisioner
    {
        readonly ICassandraSessionFactory sessionFactory;
        readonly MappingConfiguration mapping;
        private readonly CassandraStorageConfiguration config;

        public CassandraSchemaProvisioner(ICassandraSessionFactory sessionFactory, MappingConfiguration mapping, CassandraStorageConfiguration config)
        {
            this.sessionFactory = sessionFactory.ThrowIfNull(nameof(sessionFactory));
            this.mapping = mapping.ThrowIfNull(nameof(mapping));
            this.config = config.ThrowIfNull(nameof(config));
        }

        public async Task ProvisionSchemaAsync(CancellationToken token)
        {
            using (var session = sessionFactory.Open())
            {
                var mapper = new Mapper(session, mapping);
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
