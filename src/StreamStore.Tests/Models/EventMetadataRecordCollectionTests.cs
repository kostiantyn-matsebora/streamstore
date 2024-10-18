using AutoFixture;
using FluentAssertions;

namespace StreamStore.Tests.Models
{
    public class EventMetadataRecordCollectionTests
    {

        [Fact]
        public void Should_BeCreateFromCollectionOfRecords()
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
