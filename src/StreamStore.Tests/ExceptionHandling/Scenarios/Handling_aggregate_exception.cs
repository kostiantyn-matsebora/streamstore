using FluentAssertions;
using StreamStore.Testing;

namespace StreamStore.Tests.ExceptionHandling
{
    public class Handling_aggregate_exception : Scenario
    {


        [Fact]
        public void When_aggregate_exception_is_created()
        {
            // Arrange
            var exception = new Exception("Non-aggregate exception");
            var aggregateException = new AggregateException(new AggregateException(new AggregateException(exception)));

            // Act
            var realException = aggregateException.GetFirstOriginalException();

            // Assert
            realException.Should().Be(exception);
        }

        [Fact]
        public void When_aggregate_exception_is_thrown()
        {
            // Arrange
            var exception = new Exception("Non-aggregate exception");
            var aggregateException = new AggregateException(new AggregateException(new AggregateException(exception)));

            // Act
            var act = () => FuncExtension.ThrowOriginalExceptionIfOccured<bool>(() => { throw aggregateException; });

            // Assert
            act.Should().Throw<Exception>().WithMessage("Non-aggregate exception");
        }


        [Fact]
        public void When_non_aggregate_exception_is_thrown()
        {
            // Arrange
            var exception = new Exception("Non-aggregate exception");

            // Act
            var act = () => FuncExtension.ThrowOriginalExceptionIfOccured<bool>(() => { throw exception; });

            // Assert
            act.Should().Throw<Exception>().WithMessage("Non-aggregate exception");
        }
    }
}
