using Cassandra;
using FluentAssertions;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Multitenancy.TenantClusterRegistry
{
    public class Gettting_cluster : Scenario
    {

        [Fact]
        public void When_getting_cluster_with_default_configuration()
        {

            // Arrange
            Action<Builder> configure = (builder) => builder.AddContactPoint("localhost").WithDefaultKeyspace("default_keyspace");
            var registry = new CassandraTenantClusterRegistry(new DelegateClusterConfigurator().AddConfigurator(configure), new DelegateTenantClusterConfigurator());

            // Act
            var cluster = registry.GetCluster(Generated.Id);

            // Assert
            cluster.Should().NotBeNull();
            cluster.Configuration.ClientOptions.DefaultKeyspace.Should().Be("default_keyspace");

        }

        [Fact]
        public void When_getting_cluster_with_custom_tenant_configuration()
        {

            // Arrange
            var tenant = Generated.Id;
            Action<Builder> configure = (builder) => builder.AddContactPoint("localhost").WithDefaultKeyspace("default_keyspace");
          
            var configurator = new DelegateTenantClusterConfigurator((tenantId, builder) =>
                {
                    if (tenantId == tenant)
                        builder.WithDefaultKeyspace("custom_keyspace");
                }
            );

            var registry = new CassandraTenantClusterRegistry(new DelegateClusterConfigurator().AddConfigurator(configure), configurator);

            // Act
            var cluster = registry.GetCluster(tenant);

            // Assert
            cluster.Should().NotBeNull();
            cluster.Configuration.ClientOptions.DefaultKeyspace.Should().Be("custom_keyspace");

            // Act
            cluster = registry.GetCluster(Generated.Id);

            // Assert
            cluster.Should().NotBeNull();
            cluster.Configuration.ClientOptions.DefaultKeyspace.Should().Be("default_keyspace");
        }
    }
}
