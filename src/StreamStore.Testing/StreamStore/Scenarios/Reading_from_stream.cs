using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using StreamStore.Exceptions;
using StreamStore.Models;

namespace StreamStore.Testing.StreamStore.Scenarios
{
    public abstract class Reading_from_stream<TEnvironment> : StreamStoreScenario<TEnvironment> where TEnvironment : StreamStoreTestEnvironmentBase, new()
    {
        protected Reading_from_stream(TEnvironment environment) : base(environment)
        {
        }

        [Fact]
        public async Task When_stream_is_not_found()
        {
            // Arrange
            var streamId = Generated.Primitives.Id;
            IStreamStore store = Environment.Store;

            // Act
            var act = async () => await store.BeginReadAsync(streamId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<StreamNotFoundException>();
        }

        [Fact]
        public async Task When_stream_id_is_null()
        {
            // Arrange
            IStreamStore store = Environment.Store;

            // Act
            var act = () => store.BeginReadAsync(null!);
           

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task When_revision_is_less_than_one()
        {
            // Arrange
            var stream = Environment.Container.RandomStream;
            IStreamStore store = Environment.Store;

            // Act
            var act = () => store.BeginReadAsync(stream.Id, Revision.Zero);

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }

        [Fact]
        public async Task When_start_from_greater_than_actual_revision()
        {
            //Arrange
            var stream = Environment.Container.RandomStream;
            IStreamStore store = Environment.Store;

            // Act
            var act = () => store.BeginReadAsync(stream.Id, stream.Revision.Next());

            // Assert
            await act.Should().ThrowAsync<InvalidStartFromException>();
        }

        [Fact]
        public async Task When_stream_exists()
        {
            // Arrange
            var stream = Environment.Container.RandomStream;
            IStreamStore store = Environment.Store;

            // Act
            var result = await store.ReadToEndAsync(stream.Id);

            // Assert
            result.Should().NotBeNull();
            result.Last().Revision.Should().Be(stream.Revision);
            result.Should().HaveCount(stream.Revision);
            result.Should().BeInAscendingOrder(e => e.Revision);
            result.Select(e => e.Id).Should().BeEquivalentTo(stream.Events.Select(e => e.Id));
        }


        [Fact]
        public async Task When_iterating_events()
        {
            // Arrange
            var stream = Environment.Container.RandomStream;
            IStreamStore store = Environment.Store;
            List<IStreamEvent> result = new List<IStreamEvent>();

            // Act
            await foreach(var @event in await store.BeginReadAsync(stream.Id, CancellationToken.None))
            {
                // Assert
                @event.Should().NotBeNull();
                result.Add(@event);
            }

            // Assert
            result.Should().NotBeNull();
            result.Last().Revision.Should().Be(stream.Revision);
            result.Should().HaveCount(stream.Revision);
            result.Should().BeInAscendingOrder(e => e.Revision);
            result.Select(e => e.Id).Should().BeEquivalentTo(stream.Events.Select(e => e.Id));
        }
    }
}
