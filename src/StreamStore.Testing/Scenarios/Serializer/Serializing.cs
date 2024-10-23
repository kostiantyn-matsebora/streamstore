
using System.Text;
using FluentAssertions;

namespace StreamStore.Testing.Scenarios.Serializer
{
    public abstract class Serializing<TSuite> : SerializerScenario<TSuite> where TSuite : SerializerSuiteBase
    {
        protected Serializing(TSuite suite) : base(suite)
        {
        }

        [SkippableFact]
        public void When_parameters_are_absent_or_incorrect() {
            TrySkip();

            // Arrange

            // Act && Assert
            var act = () => Serializer.Serialize(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        public void When_event_is_valid()
        {
            TrySkip();

            // Arrange
            var @event = Generated.Event;

            // Act
            var serialized = Serializer.Serialize(DeserializedEvent);

            // Assert
            serialized.Should().NotBeNullOrEmpty();
            serialized.Should().BeEquivalentTo(SerializedEvent);
        }
    }
}
