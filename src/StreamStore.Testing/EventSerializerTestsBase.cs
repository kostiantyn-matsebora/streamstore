using AutoFixture;
using FluentAssertions;


namespace StreamStore.Testing
{
    public abstract class EventSerializerTestsBase
    {
        readonly IEventSerializer eventSerializer;

        protected EventSerializerTestsBase()
        {
            eventSerializer = CreateEventSerializer();
        }

        protected abstract IEventSerializer CreateEventSerializer();

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
            // Act
            Action act = () => eventSerializer.Deserialize(GeneratedValues.ByteArray);

            // & Assert
            act.Should().Throw<Exception>();
        }
    }
}
