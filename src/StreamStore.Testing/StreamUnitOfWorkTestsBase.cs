


using FluentAssertions;


namespace StreamStore.Testing
{
    public abstract class StreamUnitOfWorkTestsBase : DatabaseTestsBase
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
  
            var uow = database!.BeginAppend(streamId);

            // Act
            uow.AddRange(GeneratedValues.CreateEventItems(3));
            var act = () => uow.SaveChangesAsync(CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();

            // Act
            uow = database!.BeginAppend(streamId, 3);
            uow.AddRange(GeneratedValues.CreateEventItems(5));
            act = () => uow.SaveChangesAsync(CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();

            // Act
            uow = database!.BeginAppend(streamId, 8);
            uow.AddRange(GeneratedValues.CreateEventItems(8));
            act = () => uow.SaveChangesAsync(CancellationToken.None);

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
