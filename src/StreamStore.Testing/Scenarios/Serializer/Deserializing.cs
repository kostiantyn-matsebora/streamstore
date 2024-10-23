using FluentAssertions;

namespace StreamStore.Testing.Scenarios.Serializer
{
    public abstract class Deserializing<TSuite> : SerializerScenario<TSuite> where TSuite : SerializerSuiteBase
    {
        protected Deserializing(TSuite suite) : base(suite)
        {

        }
        [SkippableFact]
        public void When_parameters_are_absent_or_incorrect()
        {
            TrySkip();

            // Arrange

            // Act && Assert
            var act = () => Serializer.Deserialize(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        public void When_event_is_valid()
        {
            TrySkip();

            // Act
            var deserialized = Serializer.Deserialize(SerializedEvent);

            // Assert
            deserialized.Should().NotBeNull();
            deserialized.Should().BeEquivalentTo(DeserializedEvent);
        }
    }
}
    