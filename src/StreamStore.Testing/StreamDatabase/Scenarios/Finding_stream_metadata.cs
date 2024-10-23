using FluentAssertions;

namespace StreamStore.Testing.StreamDatabase.Scenarios
{
    public abstract class Find_stream_data<TSuite> : DatabaseScenario<TSuite> where TSuite : DatabaseSuiteBase
    {
        protected Find_stream_data(TSuite suite) : base(suite)
        {
        }


        [SkippableFact]
        public async Task When_stream_does_not_exist()
        {
            TrySkip();

            var metadata = await Database.FindMetadataAsync(Generated.Id);
            metadata.Should().BeNull();
        }


        [SkippableFact]
        public async Task When_stream_does_exist()
        {
            TrySkip();

            // Arrange
            var stream = Container.GetExistingStream();

            // Act
            var metadata = await Database.FindMetadataAsync(stream.Id);

            // Assert
            metadata.Should().NotBeNull();
            metadata!.Events.Should().NotBeEmpty();
            metadata.Events.Should().BeEquivalentTo(stream.Events);
        }
    }
}
