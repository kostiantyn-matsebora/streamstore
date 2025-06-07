using StreamStore.Testing;
using StreamStore.Testing.Framework;
using StreamStore.NoSql.Cassandra;
using Cassandra;
using Microsoft.Extensions.DependencyInjection;

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

        public override void ConfigurePersistence(IServiceCollection services)
        {
            services.AddCassandra(c =>
                         c.ConfigureCluster(x => ConfigureCluster(x))
                          .ConfigureStorage(k => k.WithKeyspaceName(testStorage.Keyspace.Name)));
        }

        protected override MemoryStorage CreateContainer()
        {
            return new MemoryStorage(new MemoryStorageOptions { Capacity = 100, EventPerStream = 100 });
        }
    }
}
