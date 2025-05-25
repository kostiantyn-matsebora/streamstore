using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using StreamStore.Exceptions;


namespace StreamStore.Testing.StreamStore.Scenarios
{
    public abstract class Getting_stream_metadata<TEnvironment> :StreamStoreScenario<TEnvironment> where TEnvironment : StreamStoreTestEnvironmentBase, new ()
    {
        protected Getting_stream_metadata(TEnvironment environment) : base(environment)
        {
        }

        [Fact]
        public async Task When_stream_does_not_exist()
        {
            // Arrange
            var streamId = Generated.Primitives.Id;
            IStreamStore store = Environment.Store;

            // Act
            var act = async () => await store.GetMetadataAsync(streamId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<StreamNotFoundException>();
        }


        [Fact]
        public async Task When_stream_does_exist()
        {
            // Arrange
            var stream = Environment.Container.RandomStream;
            IStreamStore store = Environment.Store;

            // Act
            var metadata = await store.GetMetadataAsync(stream.Id, CancellationToken.None);


            // Assert
            metadata.Should().NotBeNull();
            metadata!.Id.Should().Be(stream.Id);
            metadata!.Revision.Should().Be(stream.Revision);
            metadata!.LastModified.Should().Be(stream.Events.Last().Timestamp);
        }


    }
}
