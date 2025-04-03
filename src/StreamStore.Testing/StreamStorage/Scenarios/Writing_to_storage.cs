using System;
using System.Threading.Tasks;
using FluentAssertions;
using StreamStore.Exceptions;

namespace StreamStore.Testing.StreamStorage.Scenarios
{
    public abstract class Writing_to_storage<TEnvironment> : StorageScenario<TEnvironment> where TEnvironment : StorageTestEnvironmentBase, new()
    {
        protected Writing_to_storage(TEnvironment environment) : base(environment)
        {
        }

        [SkippableFact]
        public async Task When_parameters_are_absent_or_incorrect()
        {
            TrySkip();

            // Arrange
            var eventId = Id.None;
            var stream = Container.PeekStream();

            var uow = await Storage.BeginAppendAsync(stream.Id, stream.Revision);

            // Act
            var act = async () => await uow.AppendAsync(eventId, Generated.Primitives.DateTime, Generated.Objects.ByteArray);

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();

            // Act
            act = async () => await uow.AppendAsync(Generated.Primitives.Id, DateTime.MinValue, Generated.Objects.ByteArray);

            //Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();

            // Act
            act = async () => await uow.AppendAsync(Generated.Primitives.Id, Generated.Primitives.DateTime, null!);

            //Assert
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
            var eventId = Id.None;
            var stream = Container.PeekStream();

            // Act
            var act = async () => await Storage.BeginAppendAsync(stream.Id, stream.Revision + increment);

            // Assert
            await act.Should().ThrowAsync<OptimisticConcurrencyException>();

            // Arrange
            var uow = await Storage.BeginAppendAsync(stream.Id, stream.Revision);

            await Storage.BeginAppendAsync(stream.Id, stream.Revision)
                .AppendAsync(Generated.Primitives.Id, Generated.Primitives.DateTime, Generated.Objects.ByteArray)
                .CommitAsync();

            // Act
            act = () => uow.AppendAsync(Generated.Primitives.Id, Generated.Primitives.DateTime, Generated.Objects.ByteArray).CommitAsync();

            //Assert
            await act.Should().ThrowAsync<OptimisticConcurrencyException>();
        }

        [SkippableTheory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(7)]
        public async Task When_expected_revision_less_than_actual(int increment)
        {
            TrySkip();

            // Arrange
            var eventId = Id.None;
            var stream = Container.PeekStream();

            // Act
            var act = async () => await Storage.BeginAppendAsync(stream.Id, stream.Revision - increment);

            // Assert
            await act.Should().ThrowAsync<OptimisticConcurrencyException>();

            // Arrange
            var uow = await Storage.BeginAppendAsync(stream.Id, stream.Revision);

            await Storage.BeginAppendAsync(stream.Id, stream.Revision)
                .AppendAsync(Generated.Primitives.Id, Generated.Primitives.DateTime, Generated.Objects.ByteArray)
                .CommitAsync();

            // Act
            act = () => uow.AppendAsync(Generated.Primitives.Id, Generated.Primitives.DateTime, Generated.Objects.ByteArray).CommitAsync();

            //Assert
            await act.Should().ThrowAsync<OptimisticConcurrencyException>();
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

            await Storage.BeginAppendAsync(streamId, revision)
                  .AppendAsync(Generated.Primitives.Id, Generated.Primitives.DateTime, Generated.Objects.ByteArray)
                  .AppendAsync(Generated.Primitives.Id, Generated.Primitives.DateTime, Generated.Objects.ByteArray)
                  .CommitAsync();

            // Assert
            var actualRevision = await Storage.GetActualRevision(streamId);
            actualRevision.Should().NotBeNull();
            actualRevision!.Should().Be(revision + 2);

            // Act
            await Storage.BeginAppendAsync(streamId, revision + 2)
                  .AppendAsync(Generated.Primitives.Id, Generated.Primitives.DateTime, Generated.Objects.ByteArray)
                   .CommitAsync();

            // Assert
            actualRevision = await Storage.GetActualRevision(streamId);
            actualRevision.Should().NotBeNull();
            actualRevision!.Should().Be(revision + 2 + 1);

            // Act
            await Storage.BeginAppendAsync(streamId, revision + 2 + 1)
                  .AppendAsync(Generated.Primitives.Id, Generated.Primitives.DateTime, Generated.Objects.ByteArray)
                  .AppendAsync(Generated.Primitives.Id, Generated.Primitives.DateTime, Generated.Objects.ByteArray)
                  .AppendAsync(Generated.Primitives.Id, Generated.Primitives.DateTime, Generated.Objects.ByteArray)
                  .AppendAsync(Generated.Primitives.Id, Generated.Primitives.DateTime, Generated.Objects.ByteArray)
                  .AppendAsync(Generated.Primitives.Id, Generated.Primitives.DateTime, Generated.Objects.ByteArray)
                  .CommitAsync();

            // Assert
            actualRevision = await Storage.GetActualRevision(streamId);
            actualRevision.Should().NotBeNull();
            actualRevision!.Should().Be(revision + 2 + 1 + 5);
        }
    }
}
