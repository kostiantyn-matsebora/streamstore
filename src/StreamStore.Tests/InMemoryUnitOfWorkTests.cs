

using AutoFixture;
using FluentAssertions;
using StreamStore.Exceptions;
using StreamStore.InMemory;
using StreamStore.Testing;

namespace StreamStore.Tests
{
    public class InMemoryUnitOfWorkTests
    {
        [Fact]
        public async Task SaveChangesAsync_ShouldInsertEvents()
        {
            // Arrange
            var database = new InMemoryStreamDatabase();
            var fixture = new Fixture();
            var streamId = fixture.Create<Id>();
            var events = fixture.CreateEventItems(5);


            // Act
            await database
                .BeginAppendAsync(streamId, CancellationToken.None)
                .AddRangeAsync(events, CancellationToken.None)
                .SaveChangesAsync(CancellationToken.None);

            var stream = await database.FindAsync(streamId, CancellationToken.None);

            stream.Should().NotBeNull();
            stream!.Id.Should().Be(streamId);
            stream!.Events.Should().BeEquivalentTo(events);
        }

        [Fact]
        public async Task SaveChangesAsync_EnsureACIDComplaint()
        {
            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<Id>();
            var nonExistingStreamId = fixture.Create<Id>();
            var database = new InMemoryStreamDatabase();
            // Act & Assert

            var act = async () =>
            {
                await Parallel.ForEachAsync(Enumerable.Range(1, 100), async (i, _) =>
                {
                    await
                       database
                           .BeginAppendAsync(streamId, CancellationToken.None)
                           .AddRangeAsync(fixture.CreateEventItems(100), CancellationToken.None)
                           .SaveChangesAsync(CancellationToken.None);
                });
            };

            await act.Should().ThrowAsync<OptimisticConcurrencyException>();

            var stream = await database.FindAsync(streamId, CancellationToken.None);
            var nonExistingStream = await database.FindAsync(nonExistingStreamId, CancellationToken.None);

            stream.Should().NotBeNull();
            nonExistingStream.Should().BeNull();

            stream!.Events.Should().HaveCount(100);
        }

    }
}
