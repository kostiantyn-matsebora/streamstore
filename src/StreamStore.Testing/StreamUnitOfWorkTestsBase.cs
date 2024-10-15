using FluentAssertions;

namespace StreamStore.Testing
{
    public abstract class StreamUnitOfWorkTestsBase : IntegrationTestsBase
    {
        protected StreamUnitOfWorkTestsBase(ITestSuite suite) : base(suite)
        {
        }

        [SkippableFact]
        public async Task SaveChangesAsync_Should_SequentuallyAddEvents()
        {
            TrySkip();

            // Arrange
            var database = suite.CreateDatabase();
            var streamId = GeneratedValues.String;
  
            // Act
            var act = () =>
                    database!.BeginAppendAsync(streamId)
                    .AddRangeAsync(GeneratedValues.CreateEventItems(3))
                    .SaveChangesAsync(CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();

            // Act
            act = () =>
                  database!.BeginAppendAsync(streamId,3)
                  .AddRangeAsync(GeneratedValues.CreateEventItems(5))
                  .SaveChangesAsync(CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();

            // Act
            act = () =>
                 database!.BeginAppendAsync(streamId, 8)
                 .AddRangeAsync(GeneratedValues.CreateEventItems(8))
                 .SaveChangesAsync(CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();

            // Assert
            var stream = await database!.FindAsync(streamId, CancellationToken.None);
            stream.Should().NotBeNull();
            stream!.Events.Should().HaveCount(3 + 5 + 8);
            stream!.Revision.Should().Be(3 + 5 + 8);

            // Cleanup
            await database!.DeleteAsync(streamId, CancellationToken.None);
        }
    }
}
