namespace StreamDB.Tests;

public class InMemoryEventStoreTests
{
    private readonly InMemoryEventStore eventStore;

    public InMemoryEventStoreTests()
    {
        eventStore = new InMemoryEventStore();
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
    public async Task SaveEventsAsync_ShouldThrowConcurrencyException()
    {
        // Arrange
        var streamId = Guid.NewGuid().ToString();
        var events = new List<EventRecord>
        {
            new EventRecord { Id = "EventId1", Data = "Event1", Timestamp = DateTime.Now, Revision = 1 },
            new EventRecord { Id = "EventId2", Data = "Event2", Timestamp = DateTime.Now, Revision = 2 },
            new EventRecord { Id = "EventId3",  Data = "Event3", Timestamp = DateTime.Now, Revision = 3 }
        };
        await eventStore.InsertAsync(streamId, GenerateEventEntities(3,1), CancellationToken.None);
       

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
