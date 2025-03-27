using FluentAssertions;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing;
using Cassandra;
namespace StreamStore.NoSql.Tests.Cassandra.Configuration.TenantClusterConfigurator
{
    public class Configuring_cluster: Scenario
    {
        [Fact]
        public void When_configuration_delegate_not_set()
        {

            // Act
            var act = () => new DelegateTenantClusterConfigurator(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_configuration_delegate_set()
        {
            // Arrange
            Action<Id, Builder> configure = (tenantId, builder) => builder.AddContactPoint("localhost").WithDefaultKeyspace("default_keyspace");
            var configurator = new DelegateTenantClusterConfigurator(configure);
            var tenantId = Generated.Primitives.Id;
            var builder = Cluster.Builder();

            // Act
            configurator.Configure(tenantId, builder);
            var cluster = builder.Build();

            // Assert
            cluster.Should().NotBeNull();
            cluster.Configuration.ClientOptions.DefaultKeyspace.Should().Be("default_keyspace");
        }
    }
}
