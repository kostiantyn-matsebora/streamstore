﻿using FluentAssertions;
using StreamStore.Testing;

namespace StreamStore.Tests.Identifier
{
    public class When_comparing: Scenario<IdentifierTestSuite>
    {
        public When_comparing(): base(new IdentifierTestSuite())
        {
            
        }

        [Fact]
        public void When_comparing_different_type_values()
        {
            var expected = Generated.String;

            // Arrange
            var id = Suite.CreateId(expected);

            // Assert
            (id == expected).Should().BeTrue();
            (id == new Id(expected)).Should().BeTrue();
            (id != Generated.String).Should().BeTrue();
            (id == Generated.String).Should().BeFalse();
            (id != Suite.CreateId()).Should().BeTrue();
        }

        [Fact]
        public void When_comparing_with_equality()
        {
            // Arrange
            var expected = Generated.String;
            var id = Suite.CreateId(expected);


            // Assert
            id.Equals(expected).Should().BeTrue();
            id.Equals(new Id(expected)).Should().BeTrue();
            id.Equals(Generated.String).Should().BeFalse();
            id.Equals(Suite.CreateId()).Should().BeFalse();
            id.Equals(string.Empty).Should().BeFalse();
            id.Equals(new object()).Should().BeFalse();
            id.Equals((object)expected).Should().BeTrue();
            id.Equals((object)Generated.String).Should().BeFalse();
        }
    }
}