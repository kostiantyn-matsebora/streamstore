using AutoFixture;
using FluentAssertions;
using Moq;
using StreamStore.Exceptions;
using StreamStore.Testing;


namespace StreamStore.Tests.StreamStore
{
    public class Reading_from_stream : Scenario<StreamStoreSuite>
    {
        public Reading_from_stream() : base(new StreamStoreSuite())
        {
        }

        [Fact]
        public async Task When_stream_is_not_found()
        {
            // Arrange
            var streamId = Generated.Id;
            Suite.MockDatabase.Setup(db => db.FindAsync(streamId, It.IsAny<CancellationToken>())).ReturnsAsync((StreamRecord?)null);

            // Act
            var act = async () => await Suite.Store.BeginReadAsync(streamId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<StreamNotFoundException>();
        }

        [Fact]
        public async Task When_stream_id_is_null()
        {
            // Act
            var act = () => Suite.Store.BeginReadAsync(null!);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        public async Task When_stream_exists(int count)
        {
            // Arrange
            var streamRecord = new StreamRecord([.. Generated.EventRecords(count)]);

            var streamId = Generated.Id;
            var eventCount = streamRecord.Events.Count();
            var eventIds = streamRecord.Events.Select(e => e.Id).ToArray();

            Suite.MockDatabase.Setup(db => db.FindAsync(streamId, It.IsAny<CancellationToken>())).ReturnsAsync(streamRecord);

            // Act
            var result = await Suite.Store.ReadToEndAsync(streamId);

            // Assert
            result.Should().NotBeNull();
            result.MaxRevision.Should().Be(streamRecord.Revision);
            result.Should().HaveCount(eventCount);
            result.Should().BeInAscendingOrder(e => e.Revision);
            result.Select(e => e.EventId).Should().BeEquivalentTo(eventIds);
        }
    }
}
