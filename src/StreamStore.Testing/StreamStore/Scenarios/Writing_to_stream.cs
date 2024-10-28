using AutoFixture;
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

        [Fact]
        public async Task When_saving_changes()
        {
            // Arrange

            var eventIds = new List<Id>();
            var streamId = Generated.Id;

            var events = Generated.Events(3).ToArray();

            eventIds.AddRange(events.Select(e => e.Id));

            // Act 1
            // First way to append to stream
            await Suite.Store
                    .BeginWriteAsync(streamId, CancellationToken.None)
                        .AppendAsync(events[0].Id, events[0].Timestamp, events[0].EventObject)
                        .AppendAsync(events[1].Id, events[1].Timestamp, events[1].EventObject)
                        .AppendAsync(events[2].Id, events[2].Timestamp, events[2].EventObject)
                    .CommitAsync(CancellationToken.None);


            // Act 2
            events = Generated.Events(5).ToArray();
            eventIds.AddRange(events.Select(e => e.Id));

            // Second way to append to stream
            await Suite.Store
                    .BeginWriteAsync(streamId, 3, CancellationToken.None)
                        .AddRangeAsync(events)
                    .CommitAsync(CancellationToken.None);


            // Act 3
            events = Generated.Events(100).ToArray();
            eventIds.AddRange(events.Select(e => e.Id));

            // Third way to append to stream
            await Suite.Store
                    .WriteAsync(streamId, 8, events, CancellationToken.None);

            // Getting stream
            var collection = await Suite.Store.ReadToEndAsync(streamId, CancellationToken.None);

            // Assert
            collection.Should().NotBeNull();
            collection.MaxRevision.Should().Be(3 + 5 + 100);

            collection.Should().HaveCount(eventIds.Count);
            eventIds.Should().BeEquivalentTo(collection.Select(e => e.EventId));
        }

        [Fact]
        public async Task When_stream_was_already_updated()
        {
            // Arrange
            var fixture = new Fixture();
            var stream = Suite.Container.RandomStream;

            var records = fixture.CreateEventRecords(1, 3);
            var streamRecord = new StreamRecord(stream.Id, records);

            // Act

            // Trying to append events with wrong expected revision
            var act = async () =>
                await
                    Suite.Store
                        .BeginWriteAsync(stream.Id, CancellationToken.None)
                        .AddRangeAsync(Generated.Events(3))
                        .CommitAsync(CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<OptimisticConcurrencyException>();
        }

        [Fact]
        public async Task When_event_with_same_id_already_exists()
        {
            // Arrange
            var stream = Suite.Container.RandomStream;


            var existingEvents = stream.Events.Take(1).Select(e => new Event { Id = e.Id, EventObject = new { }, Timestamp = e.Timestamp });

            // Act
            // Trying to append existing events mixed with new ones, put existing to the end
            var mixedEvents = Generated.Events(5).Concat(existingEvents);

            var act = async () =>
                await
                    Suite.Store
                        .BeginWriteAsync(stream.Id, stream.Revision, CancellationToken.None)
                        .AddRangeAsync(mixedEvents)
                        .CommitAsync(CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateEventException>();
        }

        [Fact]
        public async Task When_stream_id_is_null()
        {
            // Act
            var act = () => Suite.Store.BeginWriteAsync(null!);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
