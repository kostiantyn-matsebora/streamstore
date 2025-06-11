using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;

namespace StreamStore.Testing.StreamStorage.Scenarios
{
    public abstract class Getting_stream_metadata<TEnvironment> : StorageScenario<TEnvironment> where TEnvironment : StorageTestEnvironmentBase, new()
    {
        protected Getting_stream_metadata(TEnvironment environment) : base(environment)
        {
        }


        [SkippableFact]
        public async Task When_stream_does_not_exist()
        {
            TrySkip();

            var metadata = await Storage.GetMetadataAsync(Generated.Primitives.Id);
            metadata.Should().BeNull();
        }


        [SkippableFact]
        public async Task When_stream_does_exist()
        {
            TrySkip();

            // Arrange
            var stream = Container.RandomStream;

            // Act
            var metadata = await Storage.GetMetadataAsync(stream.Id);

            // Assert
            metadata.Should().NotBeNull();
            metadata!.Revision.Should().Be(stream.Events.Count());
            metadata!.Id.Should().Be(stream.Id);
        }
    }
}
