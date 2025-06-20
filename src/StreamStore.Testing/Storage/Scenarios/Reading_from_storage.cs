﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using StreamStore.Exceptions.Reading;
using Xunit.Abstractions;

namespace StreamStore.Testing.StreamStorage.Scenarios
{
    public abstract class Reading_from_storage<TEnvironment> : StorageScenario<TEnvironment> where TEnvironment : StorageTestEnvironmentBase, new()
    {
        readonly ITestOutputHelper output;

        protected Reading_from_storage(TEnvironment environment, ITestOutputHelper output) : base(environment)
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
            var act = async () => await Storage.ReadAsync(Generated.Primitives.Id, Revision.One, count);

            // Assert
            await act.Should().ThrowAsync<StreamNotFoundException>();
        }

        [SkippableFact]
        public async Task When_reading_stream_fully()
        {
            TrySkip();

            // Arrange
            var stream = Container.RandomStream;

            // Act
            var result =  await Storage.ReadAsync(stream.Id, Revision.One, stream.Events.Count());

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveSameCount(stream.Events);
            result.Should().BeInAscendingOrder(e => e.Revision);
            result.First().Revision.Should().Be(Revision.One);
            result.Last().Revision.Should().Be(stream.Events.MaxRevision);
            result.First().Id.Should().Be(stream.Events.First().Id);
            result.Last().Id.Should().Be(stream.Events.Last().Id);
        }


        [SkippableTheory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task When_start_from_greater_than_last_revision(int increment)
        {
            TrySkip();
            // Arrange
            var stream = Container.RandomStream;

            // Act
            var events = await Storage.ReadAsync(stream.Id, stream.Revision + increment, 1);

            // Assert
            events.Should().BeEmpty();
        }

        [SkippableTheory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task When_start_from_is_less_than_or_equal_zero(int count)
        {
            TrySkip();

            // Arrange
            var stream = Container.RandomStream;

            // Act
            var act = async () => await Storage.ReadAsync(stream.Id, Revision.Zero, count);

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }



        [SkippableFact]
        public async Task When_count_is_less_or_equal_than_leftovers()
        {
            TrySkip();

            // Arrange
            var stream = Container.RandomStream;
            int page = stream.Revision / 3;

            // Act
            var events = await Storage.ReadAsync(stream.Id, Revision.One, page);

            // Assert
            events.Should().HaveCount(page);
            events.Should().BeInAscendingOrder(e => e.Revision);
            events.Last().Revision.Should().Be(page);

            // Act
            events = await Storage.ReadAsync(stream.Id, page + 1, page);

            // Assert
            events.Should().HaveCount(page);
            events.Should().BeInAscendingOrder(e => e.Revision);
            events.First().Revision.Should().Be(page + 1);
            events.Last().Revision.Should().Be(page * 2);

            // Act
            events = await Storage.ReadAsync(stream.Id, page * 2 + 1, page);

            // Assert
            events.Should().HaveCount(page);
            events.Should().BeInAscendingOrder(e => e.Revision);
            events.First().Revision.Should().Be(page * 2 + 1);
            events.Last().Revision.Should().Be(page * 3);
        }

        [SkippableFact]
        public async Task When_count_is_greater_than_leftovers()
        {
            TrySkip();

            // Arrange
            var stream = Container.RandomStream;
            int count = stream.Revision / 3;
            var startFrom = count * 2 + count / 2 + 1;
            var expectedCount = stream.Revision - count * 2 - count / 2;

            output.WriteLine($"Revision: {stream.Revision}");
            output.WriteLine($"Start from: {startFrom}");
            output.WriteLine($"count: {count}");
            output.WriteLine($"Expected number of events: {expectedCount}");

            // Act
            var events = await Storage.ReadAsync(stream.Id, startFrom, count);
            output.WriteLine($"Actual number of events: {events?.Count()}");

            // Assert
            events.Should().NotBeNull();
            events.Should().NotBeEmpty();
            events!.First().Revision.Should().Be(startFrom);
            events!.Last().Revision.Should().Be(stream.Revision);
            events.Should().HaveCount(expectedCount);
        }


        [SkippableFact]
        public async Task When_reading_events_with_custom_properties()
        {
            TrySkip();

            // Arrange
            var stream = Container.RandomStream;

            var @event = stream.Events.Where(e => e.CustomProperties != null && e.CustomProperties.Any()).FirstOrDefault();

            if (@event == null)
            {
              throw new InvalidOperationException("No event with custom properties found in the stream.");
            }

            // Act
            var events = await Storage.ReadAsync(stream.Id, @event.Revision, 1);

            // Assert
            events.Should().NotBeNullOrEmpty();
            events.First().CustomProperties.Should().BeEquivalentTo(stream.Events.First().CustomProperties);
        }
    }
}
