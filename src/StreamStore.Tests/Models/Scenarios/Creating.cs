using AutoFixture;
using FluentAssertions;
using StreamStore.Testing;

namespace StreamStore.Tests.Models
{
    public class Creating: Scenario
    {
        [Fact]
        public void When_created_with_collection_of_records()
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
    }
}
