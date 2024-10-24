using FluentAssertions;
using StreamStore.Testing;


namespace StreamStore.Tests.RevisionObject
{
    public class Instantiating : Scenario<RevisionTestSuite>
    {

        [Fact]
        public void When_revision_is_less_than_zero()
        {
            // Arrange
            var revision = -1;

            // Act
            var act = () => new Revision(revision);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
