using Newtonsoft.Json;
using AutoFixture;
using StreamStore.Serialization;
using FluentAssertions;

namespace StreamStore.Tests
{
    public class EventSerializerTests
    {
        private readonly EventSerializer eventSerializer;

        public EventSerializerTests()
        {
            eventSerializer = new EventSerializer();
        }

        [Fact]
        public void Serialize_ShouldThrowArgumentNullException_WhenEventIsNull()
        {
            // Act
            Action act = () => eventSerializer.Serialize(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Serialize_ShouldSerializeDeserialize_WhenEventIsValid()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var @event = fixture.Create<RootEvent>();

            // Act
            var serialized = eventSerializer.Serialize(@event);
            var deserialized = eventSerializer.Deserialize(serialized);

            // Assert
            serialized.Should().NotBeNullOrEmpty();
            deserialized.Should().NotBeNull();
            deserialized.Should().BeOfType<RootEvent>();
            deserialized.Should().BeEquivalentTo(@event);
        }

        [Fact]
        public void Deserialize_ShouldThrowArgumentNullException_WhenDataIsNull()
        {
            // Act
            Action act = () => eventSerializer.Deserialize(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Deserialize_ShouldThrowArgumentException_WhenDataIsInvalid()
        {
            // Arrange
            var invalidData = "invalid json string";

            // Act
            Action act = () => eventSerializer.Deserialize(invalidData!);

            // & Assert
            act.Should().Throw<JsonReaderException>();
       }
    }
}