using System;
using FluentAssertions;

namespace StreamStore.Testing.Serializer.Scenarios
{
    public abstract class Deserializing<TSuite> : SerializerScenario<TSuite> where TSuite : SerializerSuiteBase, new()
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

           
        [SkippableFact]
        public void When_data_is_valid()
        {
            TrySkip();

            // Act
            var deserialized = Serializer.Deserialize(SerializedEvent);

            // Assert
            deserialized.Should().NotBeNull();
            deserialized.Should().BeEquivalentTo(DeserializedEvent);
        }

        [SkippableFact]
        public void When_data_is_invalid()
        {
            TrySkip();

            // Act
            var act = () => Serializer.Deserialize(Generated.Objects.ByteArray);

            // Assert
            act.Should().Throw<Exception>();
        }
    }
}
