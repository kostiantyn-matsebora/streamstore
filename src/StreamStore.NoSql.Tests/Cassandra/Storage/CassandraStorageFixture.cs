using StreamStore.Testing;
using StreamStore.Testing.Framework;
using StreamStore.NoSql.Cassandra;
using Cassandra;

namespace StreamStore.NoSql.Tests.Cassandra.Storage
{
    public class CassandraStorageFixture : StorageFixtureBase<CassandraTestStorage>
    {
        public CassandraStorageFixture() : this(new CassandraTestStorage(KeyspaceConfiguration(), ConfigureCluster))
        {
        }

        CassandraStorageFixture(CassandraTestStorage storage) : base(storage)
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
            return new KeyspaceConfiguration(Generated.Names.Storage)
            {
                ReplicationClass = "SimpleStrategy",
                ReplicationFactor = 1
            };
        }

        public override void ConfigureStorage(ISingleTenantConfigurator configurator)
        {
            configurator.UseCassandra(c =>
                c.ConfigureCluster(x => ConfigureCluster(x))
                        .ConfigureStorage(k => k.WithKeyspaceName(testStorage.Keyspace.Name)));
        }

        protected override MemoryStorage CreateContainer()
        {
            return new MemoryStorage(new MemoryStorageOptions { Capacity = 100, EventPerStream = 100 });
        }
    }
}
