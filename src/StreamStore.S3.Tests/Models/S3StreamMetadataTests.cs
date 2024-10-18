using AutoFixture;
using FluentAssertions;
using StreamStore.S3.Models;

namespace StreamStore.S3.Tests.Models
{
    public class S3StreamMetadataTests
    {

        [Fact]
        public void ToRecord_Should_ReturnRecordWithSameMetadata()
        {
            // Arrange
            var fixture = new Fixture();
            var metadata = fixture.Create<S3StreamMetadata>();

            // Act
            var record = metadata.ToRecord();

            // Assert
            metadata.Should().HaveSameCount(record.Events);
            metadata.Select(m => m.Id).Should().BeEquivalentTo(record.Events.Select(e => e.Id));
        }
    }
}
