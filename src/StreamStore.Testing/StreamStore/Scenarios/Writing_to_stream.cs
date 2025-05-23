using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using StreamStore.Exceptions;


namespace StreamStore.Testing.StreamStore.Scenarios
{
    public abstract class Writing_to_stream<TEnvironment> : StreamStoreScenario<TEnvironment> where TEnvironment : StreamStoreTestEnvironmentBase, new()
    {
        protected Writing_to_stream(TEnvironment environment) : base(environment)
        {
        }

        [Theory]
        [InlineData(3, 5, 7)]
        [InlineData(5, 7, 11)]
        [InlineData(7, 11, 13)]
        public async Task When_saving_changes(int firstBatchCount, int secondBatchCount, int thirdBatchCount)
        {
            // Arrange
            IStreamStore store = Environment.Store;
            var eventIds = new List<Id>();
            var streamId = Generated.Primitives.Id;
            var events = Generated.EventEnvelopes.Many(firstBatchCount);
            eventIds.AddRange(events.Select(e => e.Id));


            // Act 1: First way to append to stream
            var uow = await store.BeginWriteAsync(streamId, CancellationToken.None);

            foreach (var @event in events)
            {
                await uow
                        .AppendAsync(x =>
                            x.WithId(@event.Id)
                             .Dated(@event.Timestamp)
                             .WithEvent(@event.Event)
                        );
            }

            var act = async () => await uow.SaveChangesAsync(CancellationToken.None);


            // Assert 1
            await act.Should().NotThrowAsync();

            // Arrange 2
            events = Generated.EventEnvelopes.Many(secondBatchCount);
            eventIds.AddRange(events.Select(e => e.Id));

            // Act 2: Second way to append to stream
            act = async () =>
                  await store
                    .BeginWriteAsync(streamId, firstBatchCount, CancellationToken.None)
                        .AppendRangeAsync(events)
                    .SaveChangesAsync(CancellationToken.None);

            // Assert 2
            await act.Should().NotThrowAsync();

            // Arrange 3
            events = Generated.EventEnvelopes.Many(thirdBatchCount);
            eventIds.AddRange(events.Select(e => e.Id));

            // Act 3: Third way to append to stream
            await store.WriteAsync(streamId, firstBatchCount + secondBatchCount, events, CancellationToken.None);

            // Getting stream
            var collection = await store.ReadToEndAsync(streamId, CancellationToken.None);

            // Assert 3
            collection.Should().NotBeNull();
            collection.Last().Revision.Should().Be(firstBatchCount + secondBatchCount + thirdBatchCount);

            collection.Should().HaveCount(eventIds.Count);
            eventIds.Should().BeEquivalentTo(collection.Select(e => e.Id));
        }

        [Theory]
        [InlineData(3)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task When_stream_was_already_updated(int count)
        {
            // Arrange
            IStreamStore store = Environment.Store;
            var stream = Environment.Container.RandomStream;

            // Act

            // Trying to append events with wrong expected revision = 0
            var act = async () =>
                await
                   store
                        .BeginWriteAsync(stream.Id, CancellationToken.None)
                        .AppendRangeAsync(Generated.EventEnvelopes.Many(count))
                        .SaveChangesAsync(CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<StreamAlreadyChangedException>();
        }

        [Fact]
        public async Task When_event_with_same_id_already_exists()
        {
            // Arrange
            IStreamStore store = Environment.Store;
            var stream = Environment.Container.RandomStream;


            // Act
            // Trying to append events already existing in collection
            var mixedEvents = Generated.EventEnvelopes.Many(5);
            mixedEvents = mixedEvents.Concat(mixedEvents.Take(1)).ToArray();

            var act = async () =>
                await
                    store
                        .BeginWriteAsync(stream.Id, stream.Revision, CancellationToken.None)
                        .AppendRangeAsync(mixedEvents)
                        .SaveChangesAsync(CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateEventException>();
        }

        [Fact]
        public async Task When_stream_id_is_null()
        {
            // Arrange
            IStreamStore store = Environment.Store;

            // Act
            var act = () => store.BeginWriteAsync(null!);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }



        [SkippableTheory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(7)]
        public async Task When_expected_revision_greater_than_actual(int increment)
        {
            TrySkip();

            // Arrange
            IStreamStore store = Environment.Store;
            var stream = Environment.Container.RandomStream;
            var existingEvents = stream.Events.Take(1).ToEventEnvelopes();

            // Act
            var act = async () => await store.BeginWriteAsync(stream.Id, stream.Revision + increment, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<StreamAlreadyChangedException>();

        }

        [SkippableTheory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(7)]
        public async Task When_expected_revision_less_than_actual(int increment)
        {
            TrySkip();

            // Arrange
            IStreamStore store = Environment.Store;
            var stream = Environment.Container.RandomStream;
            var existingEvents = stream.Events.Take(1).ToEventEnvelopes();

            // Act
            var act = async () => await store.BeginWriteAsync(stream.Id, stream.Revision - increment, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<StreamAlreadyChangedException>();
        }

        [SkippableTheory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task When_writing_many_times(bool isNew)
        {
            TrySkip();

            // Arrange
            IStreamStore store = Environment.Store;
            Id streamId = Generated.Primitives.Id;
            Revision revision = Revision.Zero;

            if (!isNew)
            {
                var record = Container.RandomStream;
                streamId = record.Id;
                revision = record.Revision;
            }

            // Act
            await store
                    .BeginWriteAsync(streamId, revision)
                        .AppendAsync(Generated.EventEnvelopes.Single)
                        .AppendAsync(Generated.EventEnvelopes.Single)
                    .SaveChangesAsync();

            // Assert
            var stream = await store.ReadToEndAsync(streamId);
            var actualRevision = stream.Last().Revision;
            actualRevision!.Should().Be(revision + 2);

            // Act
            await store
                    .BeginWriteAsync(streamId, actualRevision)
                        .AppendAsync(Generated.EventEnvelopes.Single)
                    .SaveChangesAsync();


            // Assert
            stream = await store.ReadToEndAsync(streamId);
            actualRevision = stream.Last().Revision;
            actualRevision!.Should().Be(revision + 2 + 1);

            // Act
            await store
                    .BeginWriteAsync(streamId, actualRevision)
                        .AppendAsync(Generated.EventEnvelopes.Single)
                        .AppendAsync(Generated.EventEnvelopes.Single)
                        .AppendAsync(Generated.EventEnvelopes.Single)
                        .AppendAsync(Generated.EventEnvelopes.Single)
                        .AppendAsync(Generated.EventEnvelopes.Single)
                    .SaveChangesAsync();

            // Assert
            stream = await store.ReadToEndAsync(streamId);
            actualRevision = stream.Last().Revision;
            actualRevision!.Should().Be(revision + 2 + 1 + 5);
        }
    }
}
