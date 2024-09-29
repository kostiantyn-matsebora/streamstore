

using StreamStore.InMemory;
using StreamStore;

namespace StreamStore.Tests;

public class InMemoryDatabaseTests
{
    readonly InMemoryDatabase database;

    public InMemoryDatabaseTests()
    {
        database = new InMemoryDatabase();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteStream()
    {
        // Arrange
        var streamId = Guid.NewGuid().ToString();
        await
            database
                .CreateUnitOfWork(streamId)
                .AddRange(GenerateEventEntities(3, 1))
                .SaveChangesAsync(CancellationToken.None);


        // Act
        var streamBeforeDeletion = await database.FindAsync(streamId, CancellationToken.None);
        await database.DeleteAsync(streamId, CancellationToken.None);
        var stream = await database.FindAsync(streamId, CancellationToken.None);

        // Assert
        Assert.NotNull(streamBeforeDeletion);
        Assert.Null(stream);
    }

   

    [Fact]
    public async Task FindMetadataAsync_ShouldGetStreamMetadata()
    {
        // Arrange
        var streamId = Guid.NewGuid().ToString();
        var events = new EventRecord[]
        {
            new EventRecord { Id = "EventId2",  Data = "Event2", Timestamp = DateTime.Now, Revision = 2 },
            new EventRecord { Id = "EventId1",  Data = "Event1", Timestamp = DateTime.Now , Revision = 1 }
        };

        // Act
        await
            database
                .CreateUnitOfWork(streamId)
                .AddRange(events)
                .SaveChangesAsync(CancellationToken.None);

        var stream = await database.FindMetadataAsync(streamId, CancellationToken.None);

        // Assert
        Assert.NotNull(stream);
        Assert.Equal("EventId1", stream.Events[1].Id);
        Assert.Equal("EventId2", stream.Events[0].Id);

    }

    [Fact]
    public async Task DeleteAsync_EnsureIdempotent()
    {
        // Arrange
        var streamId = Guid.NewGuid().ToString();
        await
            database
                .CreateUnitOfWork(streamId)
                .AddRange(GenerateEventEntities(3, 1))
                .SaveChangesAsync(CancellationToken.None);


        // Act
        var streamBeforeDeletion = await database.FindAsync(streamId, CancellationToken.None);

        await Parallel.ForEachAsync(Enumerable.Range(1, 100), async (i, _) =>
        {
            await database.DeleteAsync(streamId, CancellationToken.None);
        });

        var exception = await Record.ExceptionAsync(async () => await database.DeleteAsync(streamId, CancellationToken.None));

        var stream = await database.FindAsync(streamId, CancellationToken.None);

        // Assert
        Assert.Null(exception);
        Assert.NotNull(streamBeforeDeletion);
        Assert.Null(stream);
    }


    static EventRecord[] GenerateEventEntities(int count, int initialRevision)
    {
        return Enumerable.Range(1, count).Select(i => new EventRecord
        {
            Id = Guid.NewGuid().ToString(),
            Data = Guid.NewGuid().ToString(),
            Timestamp = DateTime.Now,
            Revision = initialRevision + i - 1
        }).ToArray();
    }
}
