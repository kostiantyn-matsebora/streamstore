using FluentAssertions;
using StreamStore.Exceptions;

namespace StreamStore.Testing.StreamDatabase.Scenarios
{
    public abstract class Writing_to_database<TSuite> : DatabaseScenario<TSuite> where TSuite : DatabaseSuiteBase, new()
    {
        protected Writing_to_database(TSuite suite) : base(suite)
        {
        }

        [SkippableFact]
        public async Task When_parameters_are_absent_or_incorrect()
        {
            TrySkip();

            // Arrange
            var eventId = Id.None;
            var stream = Container.PeekStream();

            var uow = await Database.BeginAppendAsync(stream.Id, stream.Revision);

            // Act
            var act = async () => await uow.AddAsync(eventId, Generated.DateTime, Generated.ByteArray);

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();

            // Act
            act = async () => await uow.AddAsync(Generated.Id, DateTime.MinValue, Generated.ByteArray);

            //Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();

            // Act
            act = async () => await uow.AddAsync(Generated.Id, Generated.DateTime, null!);

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
            var act = async () => await Database.BeginAppendAsync(stream.Id, stream.Revision + increment);

            // Assert
            await act.Should().ThrowAsync<OptimisticConcurrencyException>();

            // Arrange
            var uow = await Database.BeginAppendAsync(stream.Id, stream.Revision);

            await Database.BeginAppendAsync(stream.Id, stream.Revision)
                .AddAsync(Generated.Id, Generated.DateTime, Generated.ByteArray)
                .SaveChangesAsync();

            // Act
            act = () => uow.AddAsync(Generated.Id, Generated.DateTime, Generated.ByteArray).SaveChangesAsync();

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
            var act = async () => await Database.BeginAppendAsync(stream.Id, stream.Revision - increment);

            // Assert
            await act.Should().ThrowAsync<OptimisticConcurrencyException>();

            // Arrange
            var uow = await Database.BeginAppendAsync(stream.Id, stream.Revision);

            await Database.BeginAppendAsync(stream.Id, stream.Revision)
                .AddAsync(Generated.Id, Generated.DateTime, Generated.ByteArray)
                .SaveChangesAsync();

            // Act
            act = () => uow.AddAsync(Generated.Id, Generated.DateTime, Generated.ByteArray).SaveChangesAsync();

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
            Id streamId = Generated.Id;
            Revision revision = Revision.Zero;
            if (!isNew)
            {
                var stream = Container.RandomStream;
                streamId = stream.Id;
                revision = stream.Revision;
            }

            // Act

            await Database.BeginAppendAsync(streamId, revision)
                  .AddAsync(Generated.Id, Generated.DateTime, Generated.ByteArray)
                  .AddAsync(Generated.Id, Generated.DateTime, Generated.ByteArray)
                  .SaveChangesAsync();

            // Assert
            var metadata = await Database.FindMetadataAsync(streamId);
            metadata.Should().NotBeNull();
            metadata!.MaxRevision.Should().Be(revision + 2);

            // Act
            await Database.BeginAppendAsync(streamId, revision + 2)
                  .AddAsync(Generated.Id, Generated.DateTime, Generated.ByteArray)
                   .SaveChangesAsync();

            // Assert
            metadata = await Database.FindMetadataAsync(streamId);
            metadata.Should().NotBeNull();
            metadata!.MaxRevision.Should().Be(revision + 2 + 1);

            // Act
            await Database.BeginAppendAsync(streamId, revision + 2 + 1)
                  .AddAsync(Generated.Id, Generated.DateTime, Generated.ByteArray)
                  .AddAsync(Generated.Id, Generated.DateTime, Generated.ByteArray)
                  .AddAsync(Generated.Id, Generated.DateTime, Generated.ByteArray)
                  .AddAsync(Generated.Id, Generated.DateTime, Generated.ByteArray)
                  .AddAsync(Generated.Id, Generated.DateTime, Generated.ByteArray)
                  .SaveChangesAsync();

            // Assert
            metadata = await Database.FindMetadataAsync(streamId);
            metadata.Should().NotBeNull();
            metadata!.MaxRevision.Should().Be(revision + 2 + 1 + 5);
        }
    }
}
