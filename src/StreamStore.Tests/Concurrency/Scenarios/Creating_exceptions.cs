﻿using FluentAssertions;
using StreamStore.Exceptions;
using StreamStore.Exceptions.Appending;
using StreamStore.Testing;

namespace StreamStore.Tests.Concurrency
{
    public class Creating_exceptions: Scenario
    {
        [Fact]
        public void When_creating_stream_locked_exception()
        {

            // Arrange
            var streamId = new Id(Generated.Primitives.String);

            // Act
            var exception = new StreamLockedException(streamId);

            // Assert
            exception.Should().NotBeNull();
            exception.StreamId.Should().Be(streamId);
        }
    }
}
