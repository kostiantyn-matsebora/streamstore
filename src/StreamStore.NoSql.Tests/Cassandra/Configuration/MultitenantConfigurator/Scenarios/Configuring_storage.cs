using Cassandra;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Configuration.MultitenantConfigurator
{
    public class Configuring_storage : Scenario
    {


        [Fact]
        public void When_default_cluster_not_configured()
        {
            // Arrange
            var configurator = new CassandraMultitenantConfigurator();
            var services = new ServiceCollection();

            // Act
            var act = () => configurator.Configure(services);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void When_default_cluster_configured()
        {
            // Arrange
            var configurator = new CassandraMultitenantConfigurator();
            var services = new ServiceCollection();

            // Act
            configurator
                .ConfigureDefaultCluster(builder => builder.AddContactPoint("localhost").WithDefaultKeyspace("default_keyspace"))
                .WithKeyspaceProvider<FakeKeyspaceProvider>();
            configurator.Configure(services);
            var provider = services.BuildServiceProvider();

            // Assert
            var clusterConfigurator = provider.GetRequiredService<IClusterConfigurator>();
            var builder = Cluster.Builder();
            clusterConfigurator.Configure(builder);
            var cluster = builder.Build();
            cluster.Configuration.ClientOptions.DefaultKeyspace
                    .Should().Be("default_keyspace");
        }

        [Fact]
        public void When_kespace_provider_is_not_configured()
        {
            // Arrange
            var configurator = new CassandraMultitenantConfigurator();
            var services = new ServiceCollection();

            // Act
            configurator.ConfigureDefaultCluster(builder => builder.AddContactPoint("localhost").WithDefaultKeyspace("default_keyspace"));
            var act = () => configurator.Configure(services);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void When_configured_with_specific_dependencies()
        {
            // Arrange
            var configurator = new CassandraMultitenantConfigurator();
            var services = new ServiceCollection();

            // Act
            configurator
                .ConfigureDefaultCluster(builder => builder.AddContactPoint("localhost"))
                .WithStorageConfigurationProvider<FakeStorageConfigurationProvider>()
                .WithKeyspaceProvider<FakeKeyspaceProvider>()
                .ConfigureStoragePrototype(c => c.WithKeyspaceName("keyspace"));

            configurator.Configure(services);
            var provider = services.BuildServiceProvider();

            // Assert
            provider.GetRequiredService<IClusterConfigurator>().Should().NotBeNull();
            provider.GetRequiredService<ITenantClusterConfigurator>().Should().NotBeNull();
            provider.GetService<ICassandraKeyspaceProvider>()
                    .Should().NotBeNull()
                    .And.BeOfType<FakeKeyspaceProvider>();
            provider.GetRequiredService<ICassandraTenantStorageConfigurationProvider>()
                   .Should().NotBeNull()
                   .And.BeOfType<FakeStorageConfigurationProvider>();
            provider.GetRequiredService<CassandraStorageConfiguration>().Keyspace
                    .Should().Be("keyspace");
        }

        [Fact]
        public void When_tenant_keyspaces_configured_manually()
        {
            // Arrange
            var configurator = new CassandraMultitenantConfigurator();
            var services = new ServiceCollection();
            var keyspace = Generated.Primitives.String;
            var tenant = Generated.Primitives.Id;

            // Act
            configurator
                .ConfigureDefaultCluster(builder => builder.AddContactPoint("localhost"))
                .AddKeyspace(tenant, keyspace);

            configurator.Configure(services);
            var provider = services.BuildServiceProvider();

            // Assert
            var keyspaceProvider = provider.GetRequiredService<ICassandraKeyspaceProvider>();
            keyspaceProvider.GetKeyspace(tenant).Should().Be(keyspace);

            var act = () => keyspaceProvider.GetKeyspace(Generated.Primitives.Id);
            act.Should().Throw<InvalidOperationException>();
        }

        class FakeStorageConfigurationProvider : ICassandraTenantStorageConfigurationProvider
        {
            public CassandraStorageConfiguration GetConfiguration(Id tenanId)
            {
                throw new NotImplementedException();
            }
        }
    }
    class FakeKeyspaceProvider : ICassandraKeyspaceProvider
    {
        public string GetKeyspace(Id tenantId)
        {
            throw new NotImplementedException();
        }
    }
}
