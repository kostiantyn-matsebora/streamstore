

using AutoFixture;
using StreamStore.InMemory;

namespace StreamStore.Tests
{
    public class InMemoryUnitOfWorkTests
    {
        readonly InMemoryDatabase database;

        public InMemoryUnitOfWorkTests()
        {
            database = new InMemoryDatabase();
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldInsertEvents()
        {
            // Arrange
            var streamId = Guid.NewGuid().ToString();
            var events = new EventRecord[]
            {
            new EventRecord { Id = "EventId1",  Data = "Event1", Timestamp = DateTime.Now, Revision = 1 },
            new EventRecord { Id = "EventId2",  Data = "Event2", Timestamp = DateTime.Now , Revision = 2 }
            };

            // Act
            await database.CreateUnitOfWork(streamId)
                .AddRange(events)
                .SaveChangesAsync(CancellationToken.None);

            var stream = await database.FindAsync(streamId, CancellationToken.None);

            Assert.NotNull(stream);

            Assert.Equal("Event1", stream.Events[0].Data);
            Assert.Equal("Event2", stream.Events[1].Data);
        }



        [Fact]
        public async Task InsertAsync_ShouldThrowConcurrencyException()
        {

            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();

            await
                database
                    .CreateUnitOfWork(streamId)
                    .AddRange(fixture.CreateEvents(3, 1))
                    .SaveChangesAsync(CancellationToken.None);


            // Act & Assert
            await Assert.ThrowsAsync<OptimisticConcurrencyException>(async () =>
            {
                await
                database
                    .CreateUnitOfWork(streamId, 3)
                    .AddRange(fixture.CreateEvents(3, 1))
                    .SaveChangesAsync(CancellationToken.None);
            });

            await Assert.ThrowsAsync<OptimisticConcurrencyException>(async () =>
            {
                await
                    database
                        .CreateUnitOfWork(streamId, 3)
                        .AddRange(fixture.CreateEvents(4, 2))
                        .SaveChangesAsync(CancellationToken.None);
            });

            await Assert.ThrowsAsync<OptimisticConcurrencyException>(async () =>
            {
                await
                    database
                        .CreateUnitOfWork(streamId, 3)
                        .AddRange(fixture.CreateEvents(4, 3))
                        .SaveChangesAsync(CancellationToken.None);
            });

            var exception = await Record.ExceptionAsync(async () =>
            {
                await
                   database
                       .CreateUnitOfWork(streamId, 3)
                       .AddRange(fixture.CreateEvents(1, 4))
                       .SaveChangesAsync(CancellationToken.None);
            });

            Assert.Null(exception);
        }

        [Fact]
        public async Task InsertAsync_EnsureACIDComplaint()
        {
            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();

            // Act & Assert
            await Assert.ThrowsAsync<OptimisticConcurrencyException>(async () =>
            {
                await Parallel.ForEachAsync(Enumerable.Range(1, 100), async (i, _) =>
                {
                    await
                       database
                           .CreateUnitOfWork(streamId)
                           .AddRange(fixture.CreateEvents(100, 1))
                           .SaveChangesAsync(CancellationToken.None);
                });
            });

            var stream = await database.FindAsync(streamId, CancellationToken.None);
            var nonExistingStream = await database.FindAsync(Guid.NewGuid().ToString(), CancellationToken.None);

            Assert.NotNull(stream);
            Assert.Null(nonExistingStream);
            Assert.Equal(100, stream.Events.Length);
        }

    }
}
