using Cassandra;
using StreamStore.Testing;
using StreamStore.NoSql.Cassandra.Extensions;
using FluentAssertions;
using System.Security.Authentication;

namespace StreamStore.NoSql.Tests.Cassandra.Configuration.ClusterBuilder
{
    public class Configuring_by_cosmosdb_connection_string: Scenario
    {
        [Fact]
        public void When_connection_attributes_are_not_set()
        {
            // Arrange
            var connectionString = "Username=streamstore;Password=xxx;Port=10350";

            //Act
            var act = () => Cluster.Builder().WithCosmosDbConnectionString(connectionString);

            // Assert
            act.Should().Throw<ArgumentException>();


            // Arrange
            connectionString = "Hostname=somehost.name;Password=xxx;Port=10350";

            //Act
            act = () => Cluster.Builder().WithCosmosDbConnectionString(connectionString);

            // Assert
            act.Should().Throw<ArgumentException>();

            // Arrange
            connectionString = "Hostname=somehost.name;Username=streamstore;";

            //Act
            act = () => Cluster.Builder().WithCosmosDbConnectionString(connectionString);

            // Assert
            act.Should().Throw<ArgumentException>();


            // Arrange
            connectionString = "Hostname=somehost.name;Username=streamstore;Password=xxx;";

            //Act
            act = () => Cluster.Builder().WithCosmosDbConnectionString(connectionString);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void When_connection_string_is_proper()
        {
            // Arrange
            var connectionString = "Hostname=azure.com;Username=streamstore;Password=xxx;Port=10350";

            //Act
            var cluster = Cluster.Builder().WithCosmosDbConnectionString(connectionString).Build();

            // Assert
            cluster.Should().NotBeNull();
            var clusterConfig = cluster.Configuration;
            clusterConfig.Should().NotBeNull();

            var protocolOptions = clusterConfig.ProtocolOptions;
            protocolOptions.Should().NotBeNull();
            protocolOptions.Port.Should().Be(10350);

            var sslOptions = protocolOptions.SslOptions;
            sslOptions.Should().NotBeNull();
            sslOptions.SslProtocol.Should().Be(SslProtocols.Tls12);
            sslOptions.HostNameResolver.Should().NotBeNull();
            sslOptions.RemoteCertValidationCallback.Should().NotBeNull();
        }
    }
}
