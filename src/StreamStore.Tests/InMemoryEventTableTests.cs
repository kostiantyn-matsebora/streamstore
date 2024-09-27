namespace StreamStore.Tests;

public class InMemoryEventTableTests
{
    private readonly InMemoryEventTable eventStore;

    public InMemoryEventTableTests()
    {
        eventStore = new InMemoryEventTable();
    }

    [Fact]
    public async Task InsertAsync_ShouldInsertEvents()
    {
        // Arrange
        var streamId = Guid.NewGuid().ToString();
        var events = new EventRecord[]
        {
            new EventRecord { Id = "EventId1",  Data = "Event1", Timestamp = DateTime.Now, Revision = 1 },
            new EventRecord { Id = "EventId2",  Data = "Event2", Timestamp = DateTime.Now , Revision = 2 }
        };

        // Act
        await eventStore.InsertAsync(streamId, events, CancellationToken.None);
        var stream = await eventStore.FindAsync(streamId, CancellationToken.None);

        Assert.NotNull(stream);

        Assert.Equal("Event1", stream.Events[0].Data);
        Assert.Equal("Event2", stream.Events[1].Data);
    }


    [Fact]
    public async Task InsertAsync_ShouldThrowConcurrencyException()
    {
        // Arrange
        var streamId = Guid.NewGuid().ToString();

        await eventStore.InsertAsync(streamId, GenerateEventEntities(3, 1), CancellationToken.None);


        // Act & Assert
        await Assert.ThrowsAsync<OptimisticConcurrencyException>(async () =>
        {
            await eventStore.InsertAsync(streamId, GenerateEventEntities(3, 1), CancellationToken.None);
        });

        await Assert.ThrowsAsync<OptimisticConcurrencyException>(async () =>
        {
            await eventStore.InsertAsync(streamId, GenerateEventEntities(4, 2), CancellationToken.None);
        });

        await Assert.ThrowsAsync<OptimisticConcurrencyException>(async () =>
        {
            await eventStore.InsertAsync(streamId, GenerateEventEntities(4, 3), CancellationToken.None);
        });

        var exception = await Record.ExceptionAsync(async () =>
        {
            await eventStore.InsertAsync(streamId, GenerateEventEntities(1, 4), CancellationToken.None);
        });

        Assert.Null(exception);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteStream()
    {
        // Arrange
        var streamId = Guid.NewGuid().ToString();
        await eventStore.InsertAsync(streamId, GenerateEventEntities(3, 1), CancellationToken.None);


        // Act
        var streamBeforeDeletion = await eventStore.FindAsync(streamId, CancellationToken.None);
        await eventStore.DeleteAsync(streamId, CancellationToken.None);
        var stream = await eventStore.FindAsync(streamId, CancellationToken.None);

        // Assert
        Assert.NotNull(streamBeforeDeletion);
        Assert.Null(stream);
    }

    [Fact]
    public async Task InsertAsync_EnsureACIDComplaint()
    {
        // Arrange
        var streamId = Guid.NewGuid().ToString();

        // Act & Assert
        await Assert.ThrowsAsync<OptimisticConcurrencyException>(async () =>
        {
            await Parallel.ForEachAsync(Enumerable.Range(1, 100), async (i, _) =>
            {
                await eventStore.InsertAsync(streamId, GenerateEventEntities(100, 1), CancellationToken.None);
            });
        });

        var stream = await eventStore.FindAsync(streamId, CancellationToken.None);
        var nonExistingStream = await eventStore.FindAsync(Guid.NewGuid().ToString(), CancellationToken.None);

        Assert.NotNull(stream);
        Assert.Null(nonExistingStream);
        Assert.Equal(100, stream.Events.Length);
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
        await eventStore.InsertAsync(streamId, events, CancellationToken.None);
        var stream = await eventStore.FindMetadataAsync(streamId, CancellationToken.None);



        // Assert
        // Check also if the events were returned in proper order
        Assert.NotNull(stream);
        Assert.Equal("EventId1", stream.Events[0].Id);
        Assert.Equal("EventId2", stream.Events[1].Id);

    }

    [Fact]
    public async Task DeleteAsync_EnsureIdempotent()
    {
        // Arrange
        var streamId = Guid.NewGuid().ToString();
        await eventStore.InsertAsync(streamId, GenerateEventEntities(3, 1), CancellationToken.None);


        // Act
        var streamBeforeDeletion = await eventStore.FindAsync(streamId, CancellationToken.None);

        await Parallel.ForEachAsync(Enumerable.Range(1, 100), async (i, _) =>
        {
            await eventStore.DeleteAsync(streamId, CancellationToken.None);
        });

        var exception = await Record.ExceptionAsync(async () => await eventStore.DeleteAsync(streamId, CancellationToken.None));

        var stream = await eventStore.FindAsync(streamId, CancellationToken.None);

        // Assert
        Assert.Null(exception);
        Assert.NotNull(streamBeforeDeletion);
        Assert.Null(stream);
    }


    static IEnumerable<EventRecord> GenerateEventEntities(int count, int initialRevision)
    {
        return Enumerable.Range(1, count).Select(i => new EventRecord
        {
            Id = Guid.NewGuid().ToString(),
            Data = Guid.NewGuid().ToString(),
            Timestamp = DateTime.Now,
            Revision = initialRevision + i - 1
        });
    }
}
