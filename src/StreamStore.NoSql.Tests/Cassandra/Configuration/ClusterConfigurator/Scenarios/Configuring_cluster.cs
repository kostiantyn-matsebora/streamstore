using FluentAssertions;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing;
using Cassandra;
namespace StreamStore.NoSql.Tests.Cassandra.Configuration.ClusterConfigurator
{
    public class Configuring_cluster: Scenario
    {
        [Fact]
        public void When_configuration_delegate_set()
        {
            // Arrange
            Action<Builder> configure = (builder) => builder.AddContactPoint("localhost").WithDefaultKeyspace("default_keyspace");
            var configurator = new DelegateClusterConfigurator();
            configurator.AddConfigurator(configure);
            var builder = Cluster.Builder();

            // Act
            configurator.Configure(builder);
            var cluster = builder.Build();

            // Assert
            cluster.Should().NotBeNull();
            cluster.Configuration.ClientOptions.DefaultKeyspace.Should().Be("default_keyspace");
        }
    }
}
