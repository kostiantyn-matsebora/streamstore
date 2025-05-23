using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using StreamStore.Exceptions;
using StreamStore.Storage;

namespace StreamStore.Testing.StreamStorage.Scenarios
{
    public abstract class Writing_to_storage<TEnvironment> : StorageScenario<TEnvironment> where TEnvironment : StorageTestEnvironmentBase, new()
    {
        protected Writing_to_storage(TEnvironment environment) : base(environment)
        {
        }


        [SkippableTheory]
        [InlineData(1, 5)]
        [InlineData(5, 1)]
        [InlineData(7, 7)]
        public async Task When_expected_revision_greater_than_actual(int increment, int count)
        {
            TrySkip();

            // Arrange
            var eventId = Id.None;
            var stream = Container.PeekStream();

            // Act
            var act = async () => await Storage.WriteAsync(stream.Id, Generated.StreamEventRecords.Many(stream.Revision + increment, count), CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [SkippableTheory]
        [InlineData(1, 5)]
        [InlineData(5, 1)]
        [InlineData(7, 7)]
        public async Task When_expected_revision_less_than_actual(int increment, int count)
        {
            TrySkip();

            // Arrange
            var eventId = Id.None;
            var stream = Container.PeekStream();

            // Act
            var act = async () => await Storage.WriteAsync(stream.Id, Generated.StreamEventRecords.Many(stream.Revision - increment, count), CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateRevisionException>();
        }

        [SkippableTheory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task When_writing_many_times(bool isNew)
        {
            TrySkip();

            // Arrange
            Id streamId = Generated.Primitives.Id;
            Revision revision = Revision.Zero;

            if (!isNew)
            {
                var stream = Container.RandomStream;
                streamId = stream.Id;
                revision = stream.Revision;
            }

            // Act
            await Storage.WriteAsync(streamId, Generated.StreamEventRecords.Many(revision.Next(), 2), CancellationToken.None);

            // Assert
            var actualRevision = (await Storage.GetMetadata(streamId))!.Revision;
            actualRevision.Should().NotBeNull();
            actualRevision!.Should().Be(revision + 2);

            // Act
            await Storage.WriteAsync(streamId, Generated.StreamEventRecords.Many(actualRevision.Next(), 1), CancellationToken.None);

            // Assert
            actualRevision = (await Storage.GetMetadata(streamId))!.Revision;
            actualRevision.Should().NotBeNull();
            actualRevision!.Should().Be(revision + 2 + 1);

            // Act
            await Storage.WriteAsync(streamId, Generated.StreamEventRecords.Many(actualRevision.Next(), 5), CancellationToken.None);

            // Assert
            actualRevision = (await Storage.GetMetadata(streamId))!.Revision;
            actualRevision.Should().NotBeNull();
            actualRevision!.Should().Be(revision + 2 + 1 + 5);
        }

        [SkippableFact]
        public async Task When_event_with_same_id_already_exists()
        {
            TrySkip();

            // Arrange
            var stream = Container.PeekStream();

            // Act

            // Trying to append events already existing in collection
            var newEvents = Generated.StreamEventRecords.Many(stream.Revision.Next(), 5).ToArray();
            newEvents.Last().Id = stream.Events.First().Id; // make sure we have duplicate event id


            var act = async () =>
                  await Storage.WriteAsync(stream.Id, newEvents, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateEventException>();
        }


        [SkippableFact]
        public async Task When_event_with_same_revision_already_exists()
        {
            TrySkip();

            // Arrange
            var stream = Container.PeekStream();

            // Act

            // Trying to append events already existing in collection
            var newEvents = Generated.StreamEventRecords.Many(stream.Revision.Next(), 5).ToArray();
            var mixedEvents = newEvents.Concat(Generated.StreamEventRecords.Many(1)).ToArray(); // make sure we have duplicate event revision


            var act = async () =>
                  await Storage.WriteAsync(stream.Id, mixedEvents, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateRevisionException>();
        }
    }
}
