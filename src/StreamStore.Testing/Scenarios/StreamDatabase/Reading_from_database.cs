using FluentAssertions;
using StreamStore.Exceptions;
using Xunit.Abstractions;

namespace StreamStore.Testing.Scenarios.StreamDatabase
{
    public abstract class Reading_from_database<TSuite> : DatabaseScenario<TSuite> where TSuite : DatabaseSuiteBase
    {
        readonly ITestOutputHelper output;

        protected Reading_from_database(TSuite suite, ITestOutputHelper output) : base(suite)
        {
            this.output = output ?? throw new ArgumentNullException(nameof(output));
        }

        [SkippableTheory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task When_stream_does_not_exist(int count)
        {
            TrySkip();

            // Act
            var act = async () => await Database.ReadAsync(GeneratedValues.Id, Revision.One, count);

            // Assert
            await act.Should().ThrowAsync<StreamNotFoundException>();
        }


        [SkippableTheory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task When_start_from_greater_than_last_revision(int increment)
        {
            TrySkip();
            // Arrange
            var stream = Container.GetExistingStream();

            // Act
            var events =  await Database.ReadAsync(stream.Id, stream.Length + increment, 1);

            // Assert
            events.Should().BeEmpty();
        }

        [SkippableTheory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task When_start_from_is_less_than_or_equal_zero(int count)
        {
            // Arrange
            var stream = Container.GetExistingStream();

            // Act
            var act = async() => await Database.ReadAsync(stream.Id, Revision.Zero, count);

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }



        [SkippableFact]
        public async Task When_count_is_less_or_equal_than_leftovers()
        {
            TrySkip();
            
            // Arrange
            var stream = Container.GetExistingStream();
            int page = stream.Length / 3;

            // Act
            var events = await Database.ReadAsync(stream.Id, Revision.One, page);

            // Assert
            events.Should().HaveCount(page);
            events.Should().BeInAscendingOrder(e => e.Revision);
            events.Last().Revision.Should().Be(page);

            // Act
            events = await Database.ReadAsync(stream.Id, page + 1, page);

            // Assert
            events.Should().HaveCount(page);
            events.Should().BeInAscendingOrder(e => e.Revision);
            events.First().Revision.Should().Be(page + 1);
            events.Last().Revision.Should().Be(page * 2);

            // Act
            events = await Database.ReadAsync(stream.Id, (page * 2) + 1, page);

            // Assert
            events.Should().HaveCount(page);
            events.Should().BeInAscendingOrder(e => e.Revision);
            events.First().Revision.Should().Be((page * 2) + 1);
            events.Last().Revision.Should().Be(page * 3);
        }

        [SkippableFact]
        public async Task When_count_is_greater_than_leftovers()
        {
            TrySkip();

            // Arrange
            var stream = Container.GetExistingStream();
            int count = stream.Length / 3;
            var startFrom = (count * 2) + count / 2 + 1;
            var expectedCount = stream.Length - (count * 2) - (count / 2);

            output.WriteLine($"Length: {stream.Length}");
            output.WriteLine($"Start from: {startFrom}");
            output.WriteLine($"count: {count}");
            output.WriteLine($"Expected number of events: {expectedCount}");

            // Act
            var events = await Database.ReadAsync(stream.Id, startFrom, count);
            output.WriteLine($"Actual number of events: {events?.Length}");

            // Assert
            events.Should().NotBeNull();
            events.Should().NotBeEmpty();
            events.Should().HaveCount(expectedCount);
        }
    }
}
