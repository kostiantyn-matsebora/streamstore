
using FluentAssertions;
using StreamStore.Exceptions;
using StreamStore.Testing;
using StreamStore.Tests.Enumerator;

namespace StreamStore.Tests.EventEnumerable { 
    public abstract class Enumerating_events : Scenario<EnumerableTestEnvironment>
    {

        protected Enumerating_events(StreamReadingMode mode) : base(new EnumerableTestEnvironment(mode))
        {
        }

        [Fact]

        public async Task When_stream_is_not_found()
        {
            // Arrange
            var parameters = new StreamReadingParameters(Generated.Primitives.Id, Revision.One, 10);

            var enumerable = Environment.CreateEnumerable(parameters);

            // Act
            var act = async () => await enumerable.ReadToEndAsync();

            // Assert
            await act.Should().ThrowAsync<StreamNotFoundException>();
        }


        [Theory]
        [InlineData(1, 10)]
        [InlineData(1, 1)]
        [InlineData(20, 5)]
        [InlineData(10, 30)]
        public async Task When_reading_stream_to_end(int startFrom, int pageSize)
        {
            // Arrange
            var stream = Environment.Container.RandomStream;
            var parameters = new StreamReadingParameters(stream.Id, startFrom, pageSize);

            var enumerable = Environment.CreateEnumerable(parameters);

            // Act
            var events = (await enumerable.ReadToEndAsync()).ToArray();

            // Assert
            events.Should().NotBeEmpty();
            events.Length.Should().Be(stream.Revision - startFrom + 1);
            events.First().EventId.Should().Be(stream.Events.Skip(startFrom - 1).First().Id);
            events.Last().EventId.Should().Be(stream.Events.Last().Id);
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(1, 1)]
        [InlineData(20, 5)]
        [InlineData(10, 30)]
        public async Task When_iterating_stream_to_end(int startFrom, int pageSize)
        {
            // Arrange
            var stream = Environment.Container.RandomStream;

            var parameters = new StreamReadingParameters(stream.Id, startFrom, pageSize);

            var enumerable = Environment.CreateEnumerable(parameters);
            var events = new List<StreamEvent>();
            // Act
            await foreach (var _ in enumerable)
            {
                events.Add(_);
            }

            // Assert
            events.Should().NotBeEmpty();
            events.Count.Should().Be(stream.Revision - startFrom + 1);
            events.First().EventId.Should().Be(stream.Events.Skip(startFrom - 1).First().Id);
            events.Last().EventId.Should().Be(stream.Events.Last().Id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public async Task When_page_size_greater_than_stream_length(int increment)
        {
            var stream = Environment.Container.RandomStream;
            var pageSize = stream.Revision + increment;

            var parameters = new StreamReadingParameters(stream.Id, Revision.One, pageSize);

            var enumerable = Environment.CreateEnumerable(parameters);
            var events = new List<StreamEvent>();
            // Act
            await foreach (var _ in enumerable)
            {
                events.Add(_);
            }

            // Assert
            events.Should().NotBeEmpty();
            events.Count.Should().Be(stream.Revision);
            events.First().EventId.Should().Be(stream.Events.First().Id);
            events.Last().EventId.Should().Be(stream.Events.Last().Id);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, 1)]
        [InlineData(1, 10)]
        [InlineData(2, 0)]
        [InlineData(2, 1)]
        [InlineData(2, 10)]
        [InlineData(3, 0)]
        [InlineData(3, 1)]
        [InlineData(3, 10)]
        [InlineData(10, 0)]
        [InlineData(10, 1)]
        [InlineData(10, 2)]
        [InlineData(10, 3)]
        [InlineData(10, 10)]
        public async Task When_leftover_less_than_page_size(int startFrom, int pageSizeIncrement)
        {
            var stream = Environment.Container.RandomStream;
            var leftover = stream.Revision - startFrom + 1;
            var pageSize = leftover + pageSizeIncrement;

            var parameters = new StreamReadingParameters(stream.Id, startFrom, pageSize);

            var enumerable = Environment.CreateEnumerable(parameters);
            var events = new List<StreamEvent>();
            // Act
            await foreach (var _ in enumerable)
            {
                events.Add(_);
            }

            // Assert
            events.Should().NotBeEmpty();
            events.Count.Should().Be(leftover);
            events.First().EventId.Should().Be(stream.Events.Skip(startFrom - 1).First().Id);
            events.Last().EventId.Should().Be(stream.Events.Last().Id);
        }
    }
}
