using Cassandra;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.Storage;
using StreamStore.Testing;
using StreamStore.Testing.Storage.Configuration;

namespace StreamStore.NoSql.Tests.Cassandra.Configuration
{
    public class Configuring_multitenancy: MultitenancyConfiguratorScenario
    {
        protected override IMultitenancyConfigurator CreateConfigurator()
        {
            return new MultitenancyConfigurator(CassandraMode.Cassandra);
        }

        protected override void ConfigureRequiredDependencies(IServiceCollection services)
        {
            new StorageConfigurator().Configure(services);
        }

        [Fact]
        public void Configure_multitenancy_with_custom_dependencies()
        {
            // Arrange
            var configurator = (MultitenancyConfigurator)CreateConfigurator();
            var services = new ServiceCollection();
            var applicationName = Generated.Primitives.String;

            // Act
            configurator
                .WithStorageConfigurationProvider<DummyStorageConfigurationProvider>()
                .WithKeyspaceProvider<DummyKeyspaceProvider>()
                .WithTenantClusterConfigurator((id, builder) => builder.WithApplicationName(applicationName));

            configurator.Configure(services);

            // Assert
            var provider = services.BuildServiceProvider();
            
            provider.GetService<ICassandraTenantStorageConfigurationProvider>()
                .Should().NotBeNull()
                .And.BeOfType<DummyStorageConfigurationProvider>();

            provider.GetService<ICassandraKeyspaceProvider>()
               .Should().NotBeNull()
               .And.BeOfType<DummyKeyspaceProvider>();

            var clusterConfigurator = provider.GetService<ITenantClusterConfigurator>();

            clusterConfigurator.Should().NotBeNull();
            var builder = new Builder();
            clusterConfigurator.Configure(Generated.Primitives.Id, builder);
            builder.ApplicationName.Should().Be(applicationName);

        }

        class DummyStorageConfigurationProvider : ICassandraTenantStorageConfigurationProvider
        {
            public CassandraStorageConfiguration GetConfiguration(Id tenanId)
            {
                throw new NotImplementedException();
            }
        }

        internal class DummyKeyspaceProvider: ICassandraKeyspaceProvider
        {
            public string GetKeyspace(Id tenantId)
            {
                return "dummy_keyspace";
            }
        }
    }
}
