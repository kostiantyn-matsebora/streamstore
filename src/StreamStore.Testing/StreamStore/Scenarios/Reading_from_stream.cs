using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using StreamStore.Exceptions;
using StreamStore.Models;

namespace StreamStore.Testing.StreamStore.Scenarios
{
    public abstract class Reading_from_stream<TSuite> : StreamStoreScenario<TSuite> where TSuite : StreamStoreSuiteBase, new()
    {
        protected Reading_from_stream(TSuite suite) : base(suite)
        {
        }

        [Fact]
        public async Task When_stream_is_not_found()
        {
            // Arrange
            var streamId = Generated.Primitives.Id;
            IStreamStore store = Suite.Store;

            // Act
            var act = async () => await store.BeginReadAsync(streamId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<StreamNotFoundException>();
        }

        [Fact]
        public async Task When_stream_id_is_null()
        {
            // Arrange
            IStreamStore store = Suite.Store;

            // Act
            var act = () => store.BeginReadAsync(null!);
           

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task When_revision_is_less_than_one()
        {
            // Arrange
            var stream = Suite.Container.RandomStream;
            IStreamStore store = Suite.Store;

            // Act
            var act = () => store.BeginReadAsync(stream.Id, Revision.Zero);

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }

        [Fact]
        public async Task When_start_from_greater_than_actual_revision()
        {
            //Arrange
            var stream = Suite.Container.RandomStream;
            IStreamStore store = Suite.Store;

            // Act
            var act = () => store.BeginReadAsync(stream.Id, stream.Revision.Increment());

            // Assert
            await act.Should().ThrowAsync<InvalidStartFromException>();
        }

        [Fact]
        public async Task When_stream_exists()
        {
            // Arrange
            var stream = Suite.Container.RandomStream;
            IStreamStore store = Suite.Store;

            // Act
            var result = await store.ReadToEndAsync(stream.Id);

            // Assert
            result.Should().NotBeNull();
            result.MaxRevision.Should().Be(stream.Revision);
            result.Should().HaveCount(stream.Revision);
            result.Should().BeInAscendingOrder(e => e.Revision);
            result.Select(e => e.EventId).Should().BeEquivalentTo(stream.Events.Select(e => e.Id));
        }


        [Fact]
        public async Task When_iterating_events()
        {
            // Arrange
            var stream = Suite.Container.RandomStream;
            IStreamStore store = Suite.Store;
            StreamEventCollection result = new StreamEventCollection();

            // Act
            await foreach(var @event in await store.BeginReadAsync(stream.Id, CancellationToken.None))
            {
                // Assert
                @event.Should().NotBeNull();
                result.Add(@event);
            }

            // Assert
            result.Should().NotBeNull();
            result.MaxRevision.Should().Be(stream.Revision);
            result.Should().HaveCount(stream.Revision);
            result.Should().BeInAscendingOrder(e => e.Revision);
            result.Select(e => e.EventId).Should().BeEquivalentTo(stream.Events.Select(e => e.Id));
        }
    }
}
