using AutoFixture;
using FluentAssertions;
using StreamStore.Storage;
using StreamStore.Storage.Models;
using StreamStore.Testing;

namespace StreamStore.Tests.Models
{
    public class Creating: Scenario
    {
        [Fact]
        public void When_record_collection_created_with_collection_of_records()
        {
            // Arrange
            var fixture = new Fixture();
            var records = Generated.StreamEventRecords.Many(1, 10).ToArray();

            // Act
            var collection = new StreamEventMetadataRecordCollection(records);

            // Assert
            collection.Should().HaveSameCount(records);
            collection.Should().BeEquivalentTo(records);
        }

        [Fact]
        public void When_creating_stream_event_with_inproper_parameters()
        {
            // Arrange
            var fixture = new Fixture();
            var records = Generated.StreamEventRecords.Many(1, 10).ToArray();

            // Act
            var act = () => new StreamEventEnvelope(Id.None, Generated.Primitives.Revision, Generated.Primitives.DateTime, Generated.Objects.Single<object>(), Generated.Objects.Single<EventCustomProperties>());

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();

            // Act
            act = () => new StreamEventEnvelope(Generated.Primitives.Id, Generated.Primitives.Revision, default, Generated.Objects.Single<object>(), Generated.Objects.Single<EventCustomProperties>());

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();

            // Act
            act = () => new StreamEventEnvelope(Generated.Primitives.Id, Generated.Primitives.Revision, Generated.Primitives.DateTime,  null!, Generated.Objects.Single<EventCustomProperties>());

            // Assert
            act.Should().Throw<ArgumentNullException>();

            // Act
            act = () => new StreamEventEnvelope(Generated.Primitives.Id, Generated.Primitives.Revision, Generated.Primitives.DateTime, Generated.Objects.Single<object>(), null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();

        }

    }
}
