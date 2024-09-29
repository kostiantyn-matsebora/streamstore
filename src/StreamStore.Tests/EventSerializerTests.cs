using Newtonsoft.Json;
using AutoFixture;
using StreamStore.Serialization;

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
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => eventSerializer.Serialize(null));
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
            Assert.NotNull(serialized);
            Assert.NotNull(deserialized);
            Assert.Equal(@event.GetType(), deserialized.GetType());
        }

        [Fact]
        public void Deserialize_ShouldThrowArgumentNullException_WhenDataIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => eventSerializer.Deserialize(null));
        }

        [Fact]
        public void Deserialize_ShouldThrowArgumentException_WhenDataIsInvalid()
        {
            // Arrange
            var invalidData = "invalid json string";

            // Act & Assert
            Assert.Throws<JsonReaderException>(() => eventSerializer.Deserialize(invalidData));
       }

    }
}