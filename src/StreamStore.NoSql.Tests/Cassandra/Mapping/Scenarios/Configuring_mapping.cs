using Cassandra.Mapping;
using FluentAssertions;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.NoSql.Cassandra.Models;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Mapping
{
    public class Configuring_mapping: Scenario
    {

        [Fact]
        public void When_configuring_mapping()
        {
            // Arrange
            var config = new CassandraStorageConfiguration();

            // Act
            var mapping = new MappingConfiguration().Define(new CassandraStreamMapping(config));

            // Assert
            mapping.Get<EventEntity>().TableName.Should().Be(config.EventsTableName);
            mapping.Get<RevisionStreamEntity>().TableName.Should().Be(config.EventsTableName);

            mapping.Get<EventEntity>().PartitionKeys.Should().Contain("stream_id");
            mapping.Get<EventEntity>().GetColumnDefinition(typeof(EventEntity).GetProperty(nameof(EventEntity.StreamId))).Should().NotBeNull();
            mapping.Get<EventEntity>().GetColumnDefinition(typeof(EventEntity).GetProperty(nameof(EventEntity.Id))).Should().NotBeNull();
            mapping.Get<EventEntity>().GetColumnDefinition(typeof(EventEntity).GetProperty(nameof(EventEntity.Revision))).Should().NotBeNull();
            mapping.Get<EventEntity>().GetColumnDefinition(typeof(EventEntity).GetProperty(nameof(EventEntity.Data))).Should().NotBeNull();
            mapping.Get<EventEntity>().GetColumnDefinition(typeof(EventEntity).GetProperty(nameof(EventEntity.Timestamp))).Should().NotBeNull();
           
            mapping.Get<RevisionStreamEntity>().PartitionKeys.Should().Contain("stream_id");
            mapping.Get<RevisionStreamEntity>().GetColumnDefinition(typeof(EventEntity).GetProperty(nameof(RevisionStreamEntity.StreamId))).Should().NotBeNull();
            mapping.Get<RevisionStreamEntity>().GetColumnDefinition(typeof(EventEntity).GetProperty(nameof(RevisionStreamEntity.Revision))).Should().NotBeNull();
        }
    }
}
