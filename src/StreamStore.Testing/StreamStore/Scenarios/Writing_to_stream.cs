﻿using AutoFixture;
using FluentAssertions;
using StreamStore.Exceptions;
using StreamStore.Testing.Models;

namespace StreamStore.Testing.StreamStore.Scenarios
{
    public abstract class Writing_to_stream<TSuite> : StreamStoreScenario<TSuite> where TSuite : StreamStoreSuiteBase, new()
    {
        protected Writing_to_stream(TSuite suite) : base(suite)
        {
        }

        [Theory]
        [InlineData(3, 5, 7)]
        [InlineData(5, 7, 11)]
        [InlineData(7, 11, 13)]
        public async Task When_saving_changes(int firstBatchCount, int secondBatchCount, int thirdBatchCount)
        {
            // Arrange
            IStreamStore store = Suite.Store;
            var eventIds = new List<Id>();
            var streamId = Generated.Id;
            var events = Generated.Events(firstBatchCount);
            eventIds.AddRange(events.Select(e => e.Id));
            Revision actualRevision;

            // Act 1: First way to append to stream
            var writer = await store.BeginWriteAsync(streamId, CancellationToken.None);

            foreach (var @event in events)
            {
                await writer
                        .AppendEventAsync(x => 
                            x.WithId(@event.Id)
                             .Dated(@event.Timestamp)
                             .WithEvent(@event.EventObject)
                        );
            }

            actualRevision = await writer.CommitAsync(CancellationToken.None);

            // Assert 1
            actualRevision.Should().Be(firstBatchCount);

            // Arrange 2
            events = Generated.Events(secondBatchCount);
            eventIds.AddRange(events.Select(e => e.Id));

            // Act 2: Second way to append to stream
            actualRevision =
                await store
                    .BeginWriteAsync(streamId, actualRevision, CancellationToken.None)
                        .AppendRangeAsync(events)
                    .CommitAsync(CancellationToken.None);

            // Assert 2
            actualRevision.Should().Be(firstBatchCount + secondBatchCount);

            // Arrange 3
            events = Generated.Events(thirdBatchCount);
            eventIds.AddRange(events.Select(e => e.Id));

            // Act 3: Third way to append to stream
            await store.WriteAsync(streamId, actualRevision, events, CancellationToken.None);

            // Getting stream
            var collection = await store.ReadToEndAsync(streamId, CancellationToken.None);

            // Assert 3
            collection.Should().NotBeNull();
            collection.MaxRevision.Should().Be(firstBatchCount + secondBatchCount + thirdBatchCount);

            collection.Should().HaveCount(eventIds.Count);
            eventIds.Should().BeEquivalentTo(collection.Select(e => e.EventId));
        }

        [Theory]
        [InlineData(3)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task When_stream_was_already_updated(int count)
        {
            // Arrange
            IStreamStore store = Suite.Store;
            var stream = Suite.Container.RandomStream;

            // Act

            // Trying to append events with wrong expected revision = 0
            var act = async () =>
                await
                   store
                        .BeginWriteAsync(stream.Id, CancellationToken.None)
                        .AppendRangeAsync(Generated.Events(count))
                        .CommitAsync(CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<OptimisticConcurrencyException>();
        }

        [Fact]
        public async Task When_event_with_same_id_already_exists()
        {
            // Arrange
            IStreamStore store = Suite.Store;
            var stream = Suite.Container.RandomStream;
            var existingEvents = stream.Events.Take(1).ToEvents();

            // Act
            // Trying to append existing events mixed with new ones, put existing to the end
            var mixedEvents = Generated.Events(5).Concat(existingEvents);

            var act = async () =>
                await
                    store
                        .BeginWriteAsync(stream.Id, stream.Revision, CancellationToken.None)
                        .AppendRangeAsync(mixedEvents)
                        .CommitAsync(CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateEventException>();
        }

        [Fact]
        public async Task When_stream_id_is_null()
        {
            // Arrange
            IStreamStore store = Suite.Store;

            // Act
            var act = () => store.BeginWriteAsync(null!);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
