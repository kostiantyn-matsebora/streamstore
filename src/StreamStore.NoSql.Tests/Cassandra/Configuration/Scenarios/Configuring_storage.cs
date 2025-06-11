using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.Storage;
using StreamStore.Testing;
using StreamStore.Testing.Storage.Configuration;
using StreamStore.NoSql.Cassandra;

namespace StreamStore.NoSql.Tests.Cassandra.Configuration
{
    public class Configuring_storage : StorageConfiguratorScenario
    {
        protected override IStorageConfigurator CreateConfigurator()
        {
            var configurator = new StorageConfigurator();
            configurator.ConfigureCluster(builder => builder.AddContactPoint("localhost"));
            return configurator;
        }

        [Fact]
        public void When_manually_configuring_storage()
        {
            // Arrange
            var configurator = (StorageConfigurator)CreateConfigurator();
            var services = new ServiceCollection();
            var keyspace = Generated.Primitives.String;

            // Act
            configurator.ConfigureStorage(c => 
                c.WithMode(CassandraMode.CosmosDbCassandra)
                 .WithKeyspaceName(keyspace));
            configurator.Configure(services);

            // Assert
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetService<CassandraStorageConfiguration>();
            configuration.Should().NotBeNull();
            configuration.Mode.Should().Be(CassandraMode.CosmosDbCassandra);
            configuration.Keyspace.Should().Be(keyspace);
        }

        [Fact]
        public void When_manually_configuring_cosmosdb()
        {
            // Arrange
            var configurator = (StorageConfigurator)CreateConfigurator();
            var services = new ServiceCollection();
            var connectionString = "HostName=azure.com;Username=streamstore;Password=xxxxxxxxxxx;Port=10350";

            // Act
            configurator.UseCosmosDb(connectionString);
            configurator.Configure(services);

            // Assert
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetService<CassandraStorageConfiguration>();
            configuration.Should().NotBeNull();
            configuration.Mode.Should().Be(CassandraMode.CosmosDbCassandra);
        }

        [Fact]
        public void When_configuring_cosmosdb_by_app_configuration()
        {

            // Arrange
            var services = new ServiceCollection();
            var configurator = (StorageConfigurator)CreateConfigurator();
            var connectionString = "HostName=azure.com;Username=streamstore;Password=xxxxxxxxxxx;Port=10350";

            var inMemoryConfig = new Dictionary<string, string?>() { { "ConnectionStrings:StreamStore", connectionString } };
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemoryConfig).Build();

            // Act
            configurator.UseCosmosDb(configuration);
            configurator.Configure(services);

            // Assert
            var provider = services.BuildServiceProvider();
            var result = provider.GetService<CassandraStorageConfiguration>();
            result.Should().NotBeNull();
            result.Mode.Should().Be(CassandraMode.CosmosDbCassandra);
        }

        [Fact]
        public void Configuring_by_extension()
        {
            // Arrange
            var services = new ServiceCollection();
            var tableName = Generated.Primitives.String;

            // Act
            services.AddCassandra(b => 
            b.ConfigureStorage(c => c.WithEventsTableName(tableName))
             .ConfigureCluster(c => c.AddContactPoint("localhost")));

            // Assert
            var provider = services.BuildServiceProvider();
            provider.GetService<IStreamStorage>().Should().NotBeNull();
            var configuration = provider.GetService<CassandraStorageConfiguration>();
            configuration.Should().NotBeNull();
            configuration.EventsTableName.Should().Be(tableName);
        }
    }
}
