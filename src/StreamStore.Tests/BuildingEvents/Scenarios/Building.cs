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
            builder.WithId(Generated.Primitives.Id).Dated(Generated.Primitives.DateTime);
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
            builder.Dated(Generated.Primitives.DateTime).WithEvent(Generated.Events.Single);
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
            builder.WithId(Generated.Primitives.Id).Dated(DateTime.MinValue).WithEvent(Generated.Events.Single);
            var act = () => builder.Build();

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }


        [Fact]
        public void When_all_parameters_set()
        {
            // Arrange
            var builder = new EventBuilder();
            var id = Generated.Primitives.Id;
            var timestamp = Generated.Primitives.DateTime;
            var @event = Generated.Events.Single;

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
