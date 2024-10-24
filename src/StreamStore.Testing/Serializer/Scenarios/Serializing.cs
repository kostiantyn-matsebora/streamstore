using FluentAssertions;
using Xunit.Abstractions;

namespace StreamStore.Testing.Serializer.Scenarios
{
    public abstract class Serializing<TSuite> : SerializerScenario<TSuite> where TSuite : SerializerSuiteBase, new()
    {
        readonly ITestOutputHelper output;

        protected Serializing(ITestOutputHelper output, TSuite suite) : base(suite)
        {
            this.output = output.ThrowIfNull(nameof(output));
        }

        [SkippableFact]
        public void When_parameters_are_absent_or_incorrect()
        {
            TrySkip();

            // Arrange

            // Act && Assert
            var act = () => Serializer.Serialize(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [SkippableFact]
        public void When_event_is_valid()
        {
            TrySkip();

            // Arrange
            var @event = Generated.EventItem;

            // Act
            var serialized = Serializer.Serialize(DeserializedEvent);

            // Assert
            serialized.Should().NotBeNullOrEmpty();
            serialized.Should().BeEquivalentTo(SerializedEvent);
        }

        [SkippableFact]
        public void Use_it_to_generate_binary_data_of_event() // Use it for generating binary data of the event
        {
            var data = Serializer.Serialize(DeserializedEvent);

            var dataString = $"new byte[] {{ {string.Join(" ,", data)} }};";
            output.WriteLine(dataString);
            Assert.True(true);
        }
    }
}
