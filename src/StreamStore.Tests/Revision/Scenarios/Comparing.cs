using FluentAssertions;
using StreamStore.Testing;


namespace StreamStore.Tests.RevisionObject
{
    public class Comparing : Scenario<RevisionTestEnvironment>
    {

        [Fact]
        public void When_comparing_using_operator()
        {
            // Arrange
            var revision = RevisionTestEnvironment.CreateRevision(14);
            // Assert
            (revision > 13).Should().BeTrue();
            (revision >= 13).Should().BeTrue();
            (revision < 15).Should().BeTrue();
            (revision <= 15).Should().BeTrue();
            (revision == 14).Should().BeTrue();
            (revision != 15).Should().BeTrue();
            (revision == new Revision(14)).Should().BeTrue();
            (revision != new Revision(15)).Should().BeTrue();
            (revision == 15).Should().BeFalse();
            (revision != 14).Should().BeFalse();
            (revision == new Revision(15)).Should().BeFalse();
            (revision != new Revision(14)).Should().BeFalse();
            (revision > new Revision(13)).Should().BeTrue();
            (revision >= new Revision(13)).Should().BeTrue();
            (revision < new Revision(15)).Should().BeTrue();
            (revision <= new Revision(15)).Should().BeTrue();
            (revision > new Revision(14)).Should().BeFalse();
            (revision >= new Revision(14)).Should().BeTrue();
            (revision < new Revision(14)).Should().BeFalse();
            (revision <= new Revision(14)).Should().BeTrue();
        }


        [Fact]
        public void When_comparing_using_equals()
        {
            // Arrange
            var revision = RevisionTestEnvironment.CreateRevision(10);
            // Assert
            revision.Equals(10).Should().BeTrue();

            // Assert
            revision.Equals(new Revision(10)).Should().BeTrue();


            // Assert
            revision.Equals(11).Should().BeFalse();

            // Assert
            revision.Equals(new Revision(11)).Should().BeFalse();

            // Assert
            revision.Equals(string.Empty).Should().BeFalse();

            // Assert
            revision.Equals(new object()).Should().BeFalse();

            // Assert
            revision.Equals((object)10).Should().BeTrue();

            // Assert
            revision.Equals((object)11).Should().BeFalse();
        }

        [Fact]
        public void When_comparing_using_compareto()
        {
            // Arrange
            var revision = RevisionTestEnvironment.CreateRevision(13);

            // Assert
            revision.Should<Revision>().BeGreaterThan(new Revision(12));
            revision.Should<Revision>().BeGreaterThanOrEqualTo(new Revision(12));
            revision.Should<Revision>().BeEquivalentTo(new Revision(13));
            revision.Should<Revision>().BeLessThan(new Revision(14));
            revision.Should<Revision>().BeLessThanOrEqualTo(new Revision(14));
        }

        [Fact]
        public void When_getting_has_code()
        {
            // Arrange
            var revision = RevisionTestEnvironment.CreateRevision(11);

            // Act
            var result = revision.GetHashCode();

            // Assert
            result.Should().Be(11.GetHashCode());

        }

        [Fact]
        public void When_converting_to_string()
        {
            // Arrange
            var revision = RevisionTestEnvironment.CreateRevision(12);

            // Act
            var result = revision.ToString();

            // Assert
            result.Should().Be(12.ToString());
        }
    }
}
