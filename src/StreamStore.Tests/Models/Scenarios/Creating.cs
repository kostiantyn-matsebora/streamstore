using AutoFixture;
using FluentAssertions;
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
            var records = fixture.CreateMany<EventMetadataRecord>(10);

            // Act
            var collection = new EventMetadataRecordCollection(records);

            // Assert
            collection.Should().HaveSameCount(records);
            collection.Should().BeEquivalentTo(records);
        }

        [Fact]
        public void When_creating_stream_event_with_inproper_parameters()
        {
            // Arrange
            var fixture = new Fixture();
            var records = fixture.CreateMany<EventMetadataRecord>(10);

            // Act
            var act = () => new StreamEvent(Id.None, Generated.Primitives.Revision, Generated.Primitives.DateTime, Generated.Objects.Single<object>());

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();

            // Act
            act = () => new StreamEvent(Generated.Primitives.Id, Generated.Primitives.Revision, default, Generated.Objects.Single<object>());

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();

            // Act
            act = () => new StreamEvent(Generated.Primitives.Id, Generated.Primitives.Revision, Generated.Primitives.DateTime,  null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();

        }

    }
}
