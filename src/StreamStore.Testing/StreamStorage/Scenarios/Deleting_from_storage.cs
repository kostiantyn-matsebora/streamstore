using System.Threading.Tasks;
using FluentAssertions;

namespace StreamStore.Testing.StreamStorage.Scenarios
{
    public abstract class Deleting_from_storage<TEnvironment> : StorageScenario<TEnvironment> where TEnvironment : StorageTestEnvironmentBase, new()
    {
        protected Deleting_from_storage(TEnvironment environment) : base(environment)
        {
        }

        [SkippableFact]
        public async Task When_stream_does_not_exist()
        {
            TrySkip();

            // Arrange
            var streamId = Generated.Primitives.Id;

            // Act
            var act = async () => await Storage.DeleteAsync(streamId);

            // Assert
            await act.Should().NotThrowAsync();

            var stream = await Storage.GetMetadata(streamId);
            stream.Should().BeNull();
        }

        [SkippableFact]
        public async Task When_stream_exists()
        {
            TrySkip();

            // Arrange
            var stream = Container.PeekStream();

            var metadata = await Storage.GetMetadata(stream.Id);
            metadata.Should().NotBeNull();

            // Act
            await Storage.DeleteAsync(stream.Id);

            // Assert
            metadata = await Storage.GetMetadata(stream.Id);
            metadata.Should().BeNull();
        }

        [SkippableFact]
        public async Task When_stream_deleted_many_times()
        {
            TrySkip();

            // Arrange
            var stream = Container.PeekStream();

            var actualRevision = await Storage.GetMetadata(stream.Id);
            actualRevision.Should().NotBeNull();

            // Act
            await Storage.DeleteAsync(stream.Id);

            // Assert
            actualRevision = await Storage.GetMetadata(stream.Id);
            actualRevision.Should().BeNull();

            // Act
            await Storage.DeleteAsync(stream.Id);

            // Assert
            actualRevision = await Storage.GetMetadata(stream.Id);
            actualRevision.Should().BeNull();

            // Act
            await Storage.DeleteAsync(stream.Id);

            // Assert
            actualRevision = await Storage.GetMetadata(stream.Id);
            actualRevision.Should().BeNull();
        }
    }
}
