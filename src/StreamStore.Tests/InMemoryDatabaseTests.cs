

using StreamStore.InMemory;
using AutoFixture;

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
        var fixture = new Fixture();
        var streamId = fixture.Create<string>();
        await
            database
                .CreateUnitOfWork(streamId)
                .AddRange(fixture.CreateEvents(3, 1))
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
        var fixture = new Fixture();
        var streamId = new Fixture().Create<string>();
        var events = fixture.CreateEvents(3, 2);
       

        // Act
        await
            database
                .CreateUnitOfWork(streamId)
                .AddRange(events)
                .SaveChangesAsync(CancellationToken.None);

        var stream = await database.FindMetadataAsync(streamId, CancellationToken.None);
       

        // Assert
        Assert.NotNull(stream);
        var streamEvents = stream!.Events;

        Assert.Equal(2, streamEvents[0].Revision);
        Assert.Equal(3, streamEvents[1].Revision);
        Assert.Equal(4, streamEvents[2].Revision);
        Assert.Equal(events[0].Id, streamEvents[0].Id);
        Assert.Equal(events[1].Id, streamEvents[1].Id);
        Assert.Equal(events[2].Id, streamEvents[2].Id);
    }

    [Fact]
    public async Task DeleteAsync_EnsureIdempotent()
    {
        // Arrange
        var fixture = new Fixture();
        var streamId = fixture.Create<string>();
        await
            database
                .CreateUnitOfWork(streamId)
                .AddRange(fixture.CreateEvents(3, 1))
                .SaveChangesAsync(CancellationToken.None);

        var streamBeforeDeletion = await database.FindAsync(streamId, CancellationToken.None);

        // Act
        await Parallel.ForEachAsync(Enumerable.Range(1, 100), async (i, _) =>
        {
            await database.DeleteAsync(streamId, CancellationToken.None);
        });

        var exception = await Record.ExceptionAsync(async () => await database.DeleteAsync(streamId, CancellationToken.None));

        var stream = await database.FindAsync(streamId, CancellationToken.None);

        // Assert
        Assert.NotNull(streamBeforeDeletion);
        Assert.Null(exception);
        Assert.Null(stream);
    }

}
