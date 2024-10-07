

using FluentAssertions;

namespace StreamStore.Testing
{
    public abstract class StreamDatabaseTestsBase
    {
        readonly ITestSuite suite;

        protected StreamDatabaseTestsBase(ITestSuite suite)
        {
            ArgumentNullException.ThrowIfNull(suite, nameof(suite));
            this.suite = suite;
        }

        protected virtual void TrySkip()
        {
            Skip.If(suite.CreateDatabase() == null, "Database is not set.");
        }

        [SkippableFact]
        public async Task FindAsync_ShouldNotFindIfNotExists()
        {
            TrySkip();

            // Arrange
            var database = suite.CreateDatabase();
            var streamId = RandomValues.RandomString;

            //Act
            var stream = await database!.FindAsync(streamId, CancellationToken.None);

            //Act  && Assert
            stream.Should().BeNull();
        }

        [SkippableFact]
        public async Task FindMetadataAsync_ShouldNotFindIfNotExists()
        {
            TrySkip();

            // Arrange
            var database = suite.CreateDatabase();
            var streamId = RandomValues.RandomString;

            //Act
            var stream = await database!.FindAsync(streamId, CancellationToken.None);

            //Act  && Assert
            stream.Should().BeNull();
        }

        [SkippableFact]
        public async Task FindAsync_ShouldFind()
        {
            TrySkip();

            // Arrange
            var database = suite.CreateDatabase();
            var streamId = RandomValues.RandomString;
            var events = RandomValues.CreateEventRecords(3);
            var uow = database!.BeginAppend(streamId);
            uow.AddRange(events);
            await uow.SaveChangesAsync(CancellationToken.None);

            //Act  && Assert
            await AssertStreamIsFoundAndValid(database, streamId, events);

            //Cleanup
            await database.DeleteAsync(streamId, CancellationToken.None);
        }

        [SkippableFact]
        public async Task FindMetadataAsync_ShouldFind()
        {
            TrySkip();

            // Arrange
            var database = suite.CreateDatabase();
            var streamId = RandomValues.RandomString;
            var events = RandomValues.CreateEventRecords(3);
            var uow = database!.BeginAppend(streamId);
            uow.AddRange(events);
            await uow.SaveChangesAsync(CancellationToken.None);

            //Act  && Assert
            var metadata = await database!.FindMetadataAsync(streamId, CancellationToken.None);
            AssertStreamIsValid(metadata, events);

            //Cleanup
            await database.DeleteAsync(streamId, CancellationToken.None);
        }

        [SkippableFact]
        public async Task DeleteAsync_ShouldDelete()
        {
            TrySkip();

            // Arrange
            var database = suite.CreateDatabase();
            var streamId = RandomValues.RandomString;
            var anotherStreamId = RandomValues.RandomString;
            var events = RandomValues.CreateEventRecords(3);
            var anotherEvents = RandomValues.CreateEventRecords(10);

            var uow = database!.BeginAppend(streamId);
            uow.AddRange(events);
            await uow.SaveChangesAsync(CancellationToken.None);

            //Act
            uow = database.BeginAppend(anotherStreamId);
            uow.AddRange(anotherEvents);
            await uow.SaveChangesAsync(CancellationToken.None);

            await database.DeleteAsync(streamId, CancellationToken.None);

            //Assert
            var stream = await database.FindAsync(streamId, CancellationToken.None);
            stream.Should().BeNull();

            var anotherStream = await database.FindAsync(anotherStreamId, CancellationToken.None);
            anotherStream.Should().NotBeNull();

            //Cleanup
            await database.DeleteAsync(anotherStreamId, CancellationToken.None);
        }

        static async Task AssertStreamIsFoundAndValid(IStreamDatabase database, Id streamId, EventRecord[]? events = null)
        {
            var stream = await database.FindAsync(streamId, CancellationToken.None);
            AssertStreamIsValid(stream, events);
        }

        static void AssertStreamIsValid<T>(StreamRecord<T>? stream, EventRecord[]? events = null) where T : EventMetadataRecord
        {

            stream.Should().NotBeNull();

            if (events == null) return;

            stream!.Revision.Should().Be(events.Length);
            stream.Events.Should().BeEquivalentTo(events);
        }
    }
}
