

using AutoFixture;
using FluentAssertions;
using StreamStore.InMemory;

namespace StreamStore.Tests
{
    public class InMemoryUnitOfWorkTests
    {
        readonly InMemoryStreamDatabase database;

        public InMemoryUnitOfWorkTests()
        {
            database = new InMemoryStreamDatabase();
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldInsertEvents()
        {
            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<Id>();
            var events = fixture.CreateEventRecords(5);
          

            // Act
            await database.BeginAppend(streamId)
                .AddRange(events)
                .SaveChangesAsync(CancellationToken.None);

            var stream = await database.FindAsync(streamId, CancellationToken.None);

            stream.Should().NotBeNull();
            stream!.Id.Should().Be(streamId);
            stream!.Events.Should().BeEquivalentTo(events);
        }



        [Fact]
        public async Task SaveChangesAsync_ShouldThrowConcurrencyException()
        {
            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();

            await
                database
                    .BeginAppend(streamId)
                    .AddRange(fixture.CreateEventRecords(1, 3))
                    .SaveChangesAsync(CancellationToken.None);


            // Act & Assert
            var act =  async () =>
                await
                database
                    .BeginAppend(streamId, 3)
                    .AddRange(fixture.CreateEventRecords(1, 3))
                    .SaveChangesAsync(CancellationToken.None);

            await act.Should().ThrowAsync<OptimisticConcurrencyException>();

            act = async () =>
                await
                    database
                        .BeginAppend(streamId, 3)
                        .AddRange(fixture.CreateEventRecords(2, 4))
                        .SaveChangesAsync(CancellationToken.None);

            await act.Should().ThrowAsync<OptimisticConcurrencyException>();

            act = async () =>
                await
                    database
                        .BeginAppend(streamId, 3)
                        .AddRange(fixture.CreateEventRecords(3, 4))
                        .SaveChangesAsync(CancellationToken.None);


            await act.Should().ThrowAsync<OptimisticConcurrencyException>();

            act = async () =>
               await
                   database
                       .BeginAppend(streamId, 3)
                       .AddRange(fixture.CreateEventRecords(4, 1))
                       .SaveChangesAsync(CancellationToken.None);
            
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task SaveChangesAsync_EnsureACIDComplaint()
        {
            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<Id>();
            var nonExistingStreamId = fixture.Create<Id>();

            // Act & Assert

            var act = async () =>
            {
                await Parallel.ForEachAsync(Enumerable.Range(1, 100), async (i, _) =>
                {
                    await
                       database
                           .BeginAppend(streamId)
                           .AddRange(fixture.CreateEventRecords(1, 100))
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
