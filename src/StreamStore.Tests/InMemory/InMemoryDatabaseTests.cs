

using StreamStore.InMemory;
using StreamStore.Testing;
using AutoFixture;
using FluentAssertions;
using StreamStore;

namespace StreamStore.Tests.InMemory;

public class InMemoryDatabaseTests
{
    [Fact]
    public async Task DeleteAsync_ShouldDeleteStream()
    {
        // Arrange
        var fixture = new Fixture();
        var streamId = fixture.Create<string>();
        var database = new InMemoryStreamDatabase();
        await
            database
                .BeginAppendAsync(streamId, Revision.Zero)
                .AddRangeAsync(fixture.CreateEventItems(3))
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
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(1000)]
    public async Task FindMetadataAsync_ShouldGetStreamMetadata(int eventCount)
    {
        // Arrange
        var fixture = new Fixture();
        var streamId = new Fixture().Create<Id>();
        var events = fixture.CreateEventItems(eventCount);
        var eventIds = events.Select(e => e.Id).ToArray();
        var database = new InMemoryStreamDatabase();
        // Act
        await
            database
                .BeginAppendAsync(streamId, Revision.Zero)
                .AddRangeAsync(events)
                .SaveChangesAsync(CancellationToken.None);

        var stream = await database.FindMetadataAsync(streamId, CancellationToken.None);

        // Assert
        stream.Should().NotBeNull();
        stream!.Revision.Should().Be(eventCount);
    }

    [Fact]
    public async Task DeleteAsync_EnsureIdempotent()
    {
        // Arrange
        var fixture = new Fixture();
        var streamId = fixture.Create<string>();
        var database = new InMemoryStreamDatabase();
        await
            database
                .BeginAppendAsync(streamId, Revision.Zero)
                .AddRangeAsync(fixture.CreateEventItems(3))
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
