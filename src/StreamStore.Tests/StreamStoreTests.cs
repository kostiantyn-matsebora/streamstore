
using AutoFixture;
using Moq;



namespace StreamStore.Tests
{
    public class StreamStoreTests
    {
        private readonly Mock<IEventDatabase> mockEventDatabase;
        private readonly Mock<IEventSerializer> mockEventSerializer;
        private readonly StreamStore streamStore;

        public StreamStoreTests()
        {
            mockEventDatabase = new Mock<IEventDatabase>();
            mockEventSerializer = new Mock<IEventSerializer>();
            streamStore = new StreamStore(mockEventDatabase.Object, mockEventSerializer.Object);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDeleteAsyncOnEventDatabase()
        {
            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();

            // Act
            await streamStore.DeleteAsync(streamId);

            // Assert
            mockEventDatabase.Verify(db => db.DeleteAsync(streamId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_ShouldThrowExceptionIfStreamNotFound()
        {
            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();
            mockEventDatabase.Setup(db => db.FindAsync(streamId, It.IsAny<CancellationToken>())).ReturnsAsync((StreamRecord?)null);

            // Act && Assert
            await Assert.ThrowsAsync<StreamNotFoundException>(() => streamStore.GetAsync(streamId));
        }

        [Fact]
        public async Task OpenStreamAsync_ShouldThrowArgumentNullExceptionIfStreamIdIsNull()
        {
            // Arrange
            var expectedRevision = 1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => streamStore.OpenStreamAsync(null, expectedRevision));
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowArgumentNullExceptionIfStreamIdIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => streamStore.DeleteAsync(null));
        }

        [Fact]
        public async Task GetAsync_ShouldThrowArgumentNullExceptionIfStreamIdIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => streamStore.GetAsync(null));
        }

        [Fact]
        public async Task GetAsync_ShouldReturnStreamEntityWithEvents()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var streamRecord = fixture.Create<StreamRecord>();

            var streamId = streamRecord.Id;
            var eventCount = streamRecord.Events.Length;

            mockEventDatabase.Setup(db => db.FindAsync(streamId, It.IsAny<CancellationToken>())).ReturnsAsync(streamRecord);

            // Act
            var result = await streamStore.GetAsync(streamId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventCount, result.EventEntities.Length);
            Assert.Equal(streamId, result.StreamId);
        }
    }
}

