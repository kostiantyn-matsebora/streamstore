using FluentAssertions;
using Moq;
using StreamStore.S3.Models;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Models
{
    public class S3EventMetadataTests
    {
   
        static EventMetadataRecord CreateRecord(Id id)
        {
            return new EventMetadataRecord
            {
                Id = id,
                Timestamp = GeneratedValues.DateTime,
                Revision = GeneratedValues.Int
            };
        }

        [Fact]
        public void Equals_Should_BeEquivalentById()
        {
            // Arrange
            Id id = GeneratedValues.String;
            var metadata = new S3EventMetadata(CreateRecord(id));

            // Act
            var result = metadata.Equals(new S3EventMetadata(CreateRecord(id)));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_Should_NotBeEquivalentById()
        {
            // Arrange
            Id id = GeneratedValues.String;
            var metadata = new S3EventMetadata(CreateRecord(id));

            // Act
            var result = metadata.Equals(new S3EventMetadata(CreateRecord(GeneratedValues.String)));

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_Should_BeEquivalentId()
        {
            // Arrange
            Id id = GeneratedValues.String;
            var metadata = new S3EventMetadata(CreateRecord(id));

            // Act
            var result = metadata.Equals(new Id(id));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_Should_NotBeEquivalentId()
        {
            // Arrange
            Id id = GeneratedValues.String;
            var metadata = new S3EventMetadata(CreateRecord(id));

            // Act
            var result = metadata.Equals(new Id(GeneratedValues.String));

            // Assert
            result.Should().BeFalse();
        }
    }
}
