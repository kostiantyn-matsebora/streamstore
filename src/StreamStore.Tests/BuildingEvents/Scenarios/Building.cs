﻿using System.Reflection.Metadata;
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
            var builder = new EventEnvelopeBuilder();

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
            var builder = new EventEnvelopeBuilder();

            // Act
            builder.Dated(Generated.Primitives.DateTime).WithEvent(Generated.EventEnvelopes.Single);
            var act = () => builder.Build();

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_timestamp_is_incorrect()
        {
            // Arrange
            var builder = new EventEnvelopeBuilder();

            // Act
            builder.WithId(Generated.Primitives.Id).Dated(DateTime.MinValue).WithEvent(Generated.EventEnvelopes.Single);
            var act = () => builder.Build();

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }


        [Fact]
        public void When_all_parameters_set()
        {
            // Arrange
            var builder = new EventEnvelopeBuilder();
            var id = Generated.Primitives.Id;
            var timestamp = Generated.Primitives.DateTime;
            var @event = Generated.EventEnvelopes.Single;

            var customProperties = Generated.Objects.Single<Dictionary<string, string>>();
            var customPropertyName = Generated.Primitives.String;
            var customPropertyValue = Generated.Primitives.String;

            // Act
            builder
                .WithId(id)
                .Dated(timestamp)
                .WithEvent(@event)
                .WithCustomProperty(customPropertyName, customPropertyValue)
                .WithCustomProperties(customProperties);

            var result =  builder.Build();

            // Assert
            result.Id.Should().Be(id);
            result.Timestamp.Should().Be(timestamp);
            result.Event.Should().Be(@event);
            result.CustomProperties.Should().ContainKey(customPropertyName);
            result.CustomProperties.Should().ContainValue(customPropertyValue);
            result.CustomProperties.Should().IntersectWith(customProperties);

        }
    }
}
