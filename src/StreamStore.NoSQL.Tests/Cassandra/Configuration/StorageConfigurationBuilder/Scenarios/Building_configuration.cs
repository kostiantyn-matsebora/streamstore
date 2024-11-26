using Cassandra;
using FluentAssertions;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Configuration.StorageConfigurationBuilder
{
    public class Building_configuration: Scenario
    {

        [Fact]
        public void When_building_with_defaults()
        {
            // Arrange
            var builder = new CassandraStorageConfigurationBuilder();

            // Act
            var config = builder.Build();

            // Assert
            config.Keyspace.Should().Be("streamstore");
            config.EventsTableName.Should().Be("events");
            config.WriteConsistencyLevel.Should().Be(ConsistencyLevel.All);
            config.ReadConsistencyLevel.Should().Be(ConsistencyLevel.All);
            config.SerialConsistencyLevel.Should().Be(ConsistencyLevel.Serial);
        }

        [Fact]
        public void When_building_with_custom_values()
        {
            // Arrange
            var builder = new CassandraStorageConfigurationBuilder();

            // Act
            var config = builder
                .WithKeyspaceName("keyspace")
                .WithEventsTableName("events_table")
                .WithWriteConsistencyLevel(ConsistencyLevel.LocalQuorum)
                .WithReadConsistencyLevel(ConsistencyLevel.EachQuorum)
                .WithSerialConsistencyLevel(ConsistencyLevel.LocalSerial)
                .Build();

            // Assert
            config.Keyspace.Should().Be("keyspace");
            config.EventsTableName.Should().Be("events_table");
            config.WriteConsistencyLevel.Should().Be(ConsistencyLevel.LocalQuorum);
            config.ReadConsistencyLevel.Should().Be(ConsistencyLevel.EachQuorum);
            config.SerialConsistencyLevel.Should().Be(ConsistencyLevel.LocalSerial);
        }
    }
}
