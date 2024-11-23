using Cassandra;
using FluentAssertions;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Multitenancy.StorageConfigurationProvider
{
    public class Getting_configuration: Scenario<StorageConfigurationProviderSuite>
    {

        [Fact]
        public void When_getting_tenant_configuration()
        {
            // Arrange
            var tenant = Generated.Id;
            var keyspace = Generated.String;
            var keyspaceProvider = Suite.CassandraKeyspaceProvider;
            var configuration =
                new CassandraStorageConfigurationBuilder()
                .WithKeyspaceName("custom_keyspace")
                .WithReadConsistencyLevel(ConsistencyLevel.Quorum)
                .WithWriteConsistencyLevel(ConsistencyLevel.One)
                .WithSerialConsistencyLevel(ConsistencyLevel.LocalSerial)
                .Build();

            keyspaceProvider.Setup(x => x.GetKeyspace(tenant)).Returns(keyspace);
            var provider = new DefaultCassandraStorageConfigurationProvider(Suite.CassandraKeyspaceProvider.Object, configuration);

            // Act
            var result = provider.GetStorageConfiguration(tenant);

            // Assert
            Suite.MockRepository.VerifyAll();
            result.Should().NotBeNull();
            result.Keyspace.Should().Be(keyspace);
            result.ReadConsistencyLevel.Should().Be(ConsistencyLevel.Quorum);
            result.WriteConsistencyLevel.Should().Be(ConsistencyLevel.One);
            result.SerialConsistencyLevel.Should().Be(ConsistencyLevel.LocalSerial);
        }
    }
}
