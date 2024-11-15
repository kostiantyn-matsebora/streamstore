using FluentAssertions;
using StreamStore.Testing;

namespace StreamStore.Tests.BuildingEvents
{
    public class Building : Scenario
    {
        [Fact]
        public void When_event_object_is_not_set()
        {
            // Arrange
            var builder = new EventBuilder();

            // Act
            builder.WithId(Generated.Id).Dated(Generated.DateTime);
            var act = () => builder.Build();

            // Assert
            act.Should().Throw<ArgumentNullException>();

        }

        [Fact]
        public void When_id_is_not_set()
        {
            // Arrange
            var builder = new EventBuilder();

            // Act
            builder.Dated(Generated.DateTime).WithEvent(Generated.Event);
            var act = () => builder.Build();

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_timestamp_is_incorrect()
        {
            // Arrange
            var builder = new EventBuilder();

            // Act
            builder.WithId(Generated.Id).Dated(DateTime.MinValue).WithEvent(Generated.Event);
            var act = () => builder.Build();

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }


        [Fact]
        public void When_all_parameters_set()
        {
            // Arrange
            var builder = new EventBuilder();
            var id = Generated.Id;
            var timestamp = Generated.DateTime;
            var @event = Generated.Event;

            // Act
            builder.WithId(id).Dated(timestamp).WithEvent(@event);
            var result =  builder.Build();

            // Assert
            result.Id.Should().Be(id);
            result.Timestamp.Should().Be(timestamp);
            result.EventObject.Should().Be(@event);
        }
    }
}
