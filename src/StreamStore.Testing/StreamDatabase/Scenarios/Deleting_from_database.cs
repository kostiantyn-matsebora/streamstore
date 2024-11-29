using FluentAssertions;

namespace StreamStore.Testing.StreamDatabase.Scenarios
{
    public abstract class Deleting_from_database<TSuite> : DatabaseScenario<TSuite> where TSuite : DatabaseSuiteBase, new()
    {
        protected Deleting_from_database(TSuite suite) : base(suite)
        {
        }

        [SkippableFact]
        public async Task When_stream_does_not_exist()
        {
            TrySkip();

            // Arrange
            var streamId = Generated.Id;

            // Act
            var act = async () => await Database.DeleteAsync(streamId);

            // Assert
            await act.Should().NotThrowAsync();

            var stream = await Database.GetActualRevision(streamId);
            stream.Should().BeNull();
        }

        [SkippableFact]
        public async Task When_stream_exists()
        {
            TrySkip();

            // Arrange
            var stream = Container.PeekStream();

            var actualRevision = await Database.GetActualRevision(stream.Id);
            actualRevision.Should().NotBeNull();

            // Act
            await Database.DeleteAsync(stream.Id);

            // Assert
            actualRevision = await Database.GetActualRevision(stream.Id);
            actualRevision.Should().BeNull();
        }

        [SkippableFact]
        public async Task When_stream_deleted_many_times()
        {
            TrySkip();

            // Arrange
            var stream = Container.PeekStream();

            var actualRevision = await Database.GetActualRevision(stream.Id);
            actualRevision.Should().NotBeNull();

            // Act
            await Database.DeleteAsync(stream.Id);

            // Assert
            actualRevision = await Database.GetActualRevision(stream.Id);
            actualRevision.Should().BeNull();

            // Act
            await Database.DeleteAsync(stream.Id);

            // Assert
            actualRevision = await Database.GetActualRevision(stream.Id);
            actualRevision.Should().BeNull();

            // Act
            await Database.DeleteAsync(stream.Id);

            // Assert
            actualRevision = await Database.GetActualRevision(stream.Id);
            actualRevision.Should().BeNull();
        }
    }
}
