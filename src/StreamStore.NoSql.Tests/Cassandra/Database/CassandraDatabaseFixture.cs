using StreamStore.Testing;
using StreamStore.Testing.Framework;
using StreamStore.NoSql.Cassandra;
using Cassandra;

namespace StreamStore.NoSql.Tests.Cassandra.Database
{
    public class CassandraDatabaseFixture : DatabaseFixtureBase<CassandraTestDatabase>
    {
        public CassandraDatabaseFixture() : this(new CassandraTestDatabase(KeyspaceConfiguration(), ConfigureCluster))
        {
        }

        CassandraDatabaseFixture(CassandraTestDatabase database) : base(database)
        {

        }

        static void ConfigureCluster(Builder builder)
        {
            // Put your cluster configuration here
            builder
                .AddContactPoint("localhost")
                .WithQueryTimeout(10000)
                ;
        }

        static KeyspaceConfiguration KeyspaceConfiguration()
        {
            // Put your keyspace configuration here
            return new KeyspaceConfiguration(Generated.DatabaseName)
            {
                ReplicationClass = "SimpleStrategy",
                ReplicationFactor = 1
            };
        }

        public override void ConfigureDatabase(ISingleTenantConfigurator configurator)
        {
            configurator.UseCassandra(c =>
                c.ConfigureCluster(x => ConfigureCluster(x))
                        .ConfigureStorage(k => k.WithKeyspaceName(testDatabase.Keyspace.Name)));
        }

        protected override MemoryDatabase CreateContainer()
        {
            return new MemoryDatabase(new MemoryDatabaseOptions { Capacity = 100, EventPerStream = 100 });
        }
    }
}
