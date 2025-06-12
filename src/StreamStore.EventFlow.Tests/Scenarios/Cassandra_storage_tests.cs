using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra;
using StreamStore.NoSql.Tests.Cassandra.Storage;
namespace StreamStore.EventFlow.Tests
{
    [TestFixture]
    [Ignore("Requires Cassandra server running")]
    public class Cassandra_storage_tests : TestSuiteForEventStore
    {
        protected override void ConfigureStreamStorage(IServiceCollection services, string storageName)
        {
            services.UseCassandra(c =>
                c.ConfigureStorage(x => x.WithKeyspaceName(storageName))
                 .ConfigureCluster(x =>
                    x.AddContactPoint("localhost")
                     .WithDefaultKeyspace(storageName)));
        }

        protected override void ProvisionStorage(string name)
        {
            new CassandraTestStorage(new KeyspaceConfiguration(name), c => c.AddContactPoint("localhost")).EnsureExists();
        }
    }
}
