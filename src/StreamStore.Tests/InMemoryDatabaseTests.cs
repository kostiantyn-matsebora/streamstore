

using StreamStore.InMemory;
using StreamStore.Testing;
using AutoFixture;
using FluentAssertions;

namespace StreamStore.Tests;

public class InMemoryDatabaseTests
{
    readonly InMemoryStreamDatabase database;

    public InMemoryDatabaseTests()
    {
        database = new InMemoryStreamDatabase();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteStream()
    {
        // Arrange
        var fixture = new Fixture();
        var streamId = fixture.Create<string>();
        await
            database
                .BeginAppend(streamId)
                .AddRange(fixture.CreateEventRecords(1, 3))
                .SaveChangesAsync(CancellationToken.None);

        // Act
        var streamBeforeDeletion = await database.FindAsync(streamId, CancellationToken.None);
        await database.DeleteAsync(streamId, CancellationToken.None);
        var stream = await database.FindAsync(streamId, CancellationToken.None);

        // Assert
        streamBeforeDeletion.Should().NotBeNull();
        stream.Should().BeNull();
    }

    [Theory]
    [InlineData(1,1)]
    [InlineData(1, 2)]
    [InlineData(1, 3)]
    [InlineData(2, 1)]
    [InlineData(2, 2)]
    [InlineData(2, 3)]
    [InlineData(10, 100)]
    [InlineData(100, 10)]
    [InlineData(100, 1000)]
    public async Task FindMetadataAsync_ShouldGetStreamMetadata(int initialRevision, int eventCount)
    {
        // Arrange
        var fixture = new Fixture();
        var streamId = new Fixture().Create<Id>();
        var events = fixture.CreateEventRecords(initialRevision, eventCount);
        var eventIds = events.Select(e => e.Id).ToArray();

        // Act
        await
            database
                .BeginAppend(streamId)
                .AddRange(events)
                .SaveChangesAsync(CancellationToken.None);

        var stream = await database.FindMetadataAsync(streamId, CancellationToken.None);

        // Assert
        stream.Should().NotBeNull();
        stream!.Id.Should().BeEquivalentTo(streamId);
        stream!.Revision.Should().Be(initialRevision + eventCount - 1);
        stream!.Events.Should().BeEquivalentTo(events.Cast<EventMetadataRecord>());
    }

    [Fact]
    public async Task DeleteAsync_EnsureIdempotent()
    {
        // Arrange
        var fixture = new Fixture();
        var streamId = fixture.Create<string>();
        await
            database
                .BeginAppend(streamId)
                .AddRange(fixture.CreateEventRecords(1, 3))
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
        exception.Should().BeNull();
        streamBeforeDeletion.Should().NotBeNull();
        stream.Should().BeNull();
    }

}
