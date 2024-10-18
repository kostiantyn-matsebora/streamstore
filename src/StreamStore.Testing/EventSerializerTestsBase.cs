using AutoFixture;
using FluentAssertions;
using StreamStore.Serialization;


namespace StreamStore.Testing
{
    public abstract class EventSerializerTestsBase
    {
        protected static readonly TypeRegistry registry = TypeRegistry.CreateAndInitialize();

        protected abstract IEventSerializer CreateEventSerializer(bool compression);
        protected virtual object CreateEvent()
        {
            Fixture fixture = new Fixture();
            return fixture.Create<RootEvent>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Serialize_ShouldThrowArgumentNullException_WhenEventIsNull(bool compression)
        {
            // Arrange
            var serializer = CreateEventSerializer(compression);

            // Act
            Action act = () => serializer.Serialize(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Serialize_ShouldSerializeDeserialize_WhenEventIsValid(bool compression)
        {
            // Arrange
            var @event = CreateEvent();
            var serializer = CreateEventSerializer(compression);

            // Act
            var serialized = serializer.Serialize(@event);
            var deserialized = serializer.Deserialize(serialized);

            // Assert
            serialized.Should().NotBeNullOrEmpty();
            deserialized.Should().NotBeNull();
            deserialized.Should().BeEquivalentTo(@event);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Deserialize_ShouldThrowArgumentNullException_WhenDataIsNull(bool compression)
        {

            // Arrange
            var serializer = CreateEventSerializer(compression);

            // Act
            Action act = () => serializer.Deserialize(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Deserialize_ShouldThrowArgumentException_WhenDataIsInvalid(bool compression)
        {
            // Arrange
            var serializer = CreateEventSerializer(compression);

            // Act
            Action act = () => serializer.Deserialize(GeneratedValues.ByteArray);

            // Assert
            act.Should().Throw<Exception>();
        }
    }
}
