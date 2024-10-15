using FluentAssertions;


namespace StreamStore.Tests
{
    public class RevisionTests
   {
        Revision CreateRevision(int revision = 0)
        {
            return Revision.New(revision);
        }

        [Fact]
        public void Increment_Should_CreateIncrementedInstance()
        {
            // Arrange
            var revision = this.CreateRevision();

            // Act
            var result = revision.Increment();

            // Assert
            result.Should().NotBe(revision);
            result.Value.Should().Be(revision.Value + 1);
        }

        [Fact]
        public void Equals_Should_BeEquivalent()
        {
            // Arrange
            var revision = this.CreateRevision(10);
            // Assert
            revision.Equals(10).Should().BeTrue();

            // Assert
            revision.Equals(Revision.New(10)).Should().BeTrue();

           
            // Assert
            revision.Equals(11).Should().BeFalse();

            // Assert
            revision.Equals(Revision.New(11)).Should().BeFalse();

            // Assert
            revision.Equals(null).Should().BeFalse();

            // Assert
            revision.Equals(new object()).Should().BeFalse();

            // Assert
            revision.Equals((object)10).Should().BeTrue();

            // Assert
            revision.Equals((object)11).Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_Should_ReturnCodeOfIntegerValue()
        {
            // Arrange
            var revision = this.CreateRevision(11);

            // Act
            var result = revision.GetHashCode();

            // Assert
            result.Should().Be(11.GetHashCode());
            
        }

        [Fact]
        public void ToString_Should_ReturnToStringOfIntegerValue()
        {
            // Arrange
            var revision = this.CreateRevision(12);

            // Act
            var result = revision.ToString();

            // Assert
            result.Should().Be(12.ToString());
        }

        [Fact]
        public void CompareTo_Should_CompareIntegerValue()
        {
            // Arrange
            var revision = this.CreateRevision(13);

            // Assert
            revision.Should<Revision>().BeGreaterThan(Revision.New(12));
            revision.Should<Revision>().BeGreaterThanOrEqualTo(Revision.New(12));
            revision.Should<Revision>().BeEquivalentTo(Revision.New(13));
            revision.Should<Revision>().BeLessThan(Revision.New(14));
            revision.Should<Revision>().BeLessThanOrEqualTo(Revision.New(14));

        }


        [Fact]
        public void Operator_Should_CompareIntegerValue()
        {
            // Arrange
            var revision = this.CreateRevision(14);

            // Assert
            (revision > 13).Should().BeTrue();
            (revision >= 13).Should().BeTrue();
            (revision < 15).Should().BeTrue();
            (revision <= 15).Should().BeTrue();
            (revision == 14).Should().BeTrue();
            (revision != 15).Should().BeTrue();
            (revision == Revision.New(14)).Should().BeTrue();
            (revision != Revision.New(15)).Should().BeTrue();
            (revision == 15).Should().BeFalse();
            (revision != 14).Should().BeFalse();
            (revision == Revision.New(15)).Should().BeFalse();
            (revision != Revision.New(14)).Should().BeFalse();
            (revision > Revision.New(13)).Should().BeTrue();
            (revision >= Revision.New(13)).Should().BeTrue();
            (revision < Revision.New(15)).Should().BeTrue();
            (revision <= Revision.New(15)).Should().BeTrue();
            (revision > Revision.New(14)).Should().BeFalse();
            (revision >= Revision.New(14)).Should().BeTrue();
            (revision < Revision.New(14)).Should().BeFalse();
            (revision <= Revision.New(14)).Should().BeTrue();
        }

       
    }
}
