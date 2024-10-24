using AutoFixture;
using FluentAssertions;
using Moq;
using StreamStore.Testing;


namespace StreamStore.Tests.StreamStore
{
    public class Deleting_stream: Scenario<StreamStoreSuite>
    {
        [Fact]
        public async Task When_stream_deleted()
        {
            // Arrange
            var streamId = Generated.Id;

            // Act
            await Suite.Store.DeleteAsync(streamId);

            // Assert
            Suite.MockDatabase.Verify(db => db.DeleteAsync(streamId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task When_stream_id_is_null()
        {
            // Act
            var act = () => Suite.Store.DeleteAsync(null!);

            // & Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
