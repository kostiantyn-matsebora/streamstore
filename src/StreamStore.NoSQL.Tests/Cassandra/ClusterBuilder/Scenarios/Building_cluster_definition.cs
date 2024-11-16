using StreamStore.NoSql.Cassandra;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.ClusterBuilder
{
    public class Building_cluster_definition: Scenario
    {
        [Fact]
        public async Task Build_cluster()
        {
            var clusterConfig = new CassandraClusterConfiguration();
            var dbConfig = new CassandraDatabaseConfiguration();
            var builder = new CassandraClusterBuilder(clusterConfig);
            var db = new CassandraTestDatabase(builder, dbConfig);
            var schemaProvisioner = 
                new CassandraSchemaProvisioner(
                    new CassandraSessionFactory(builder, dbConfig), 
                    new DataContextFactory(new TypeMapFactory(dbConfig)));

            db.EnsureExists();
            await schemaProvisioner.ProvisionSchemaAsync(CancellationToken.None);
        }
    }
}