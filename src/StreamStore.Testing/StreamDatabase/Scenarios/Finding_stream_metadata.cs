using FluentAssertions;

namespace StreamStore.Testing.StreamDatabase.Scenarios
{
    public abstract class Find_stream_data<TSuite> : DatabaseScenario<TSuite> where TSuite : DatabaseSuiteBase, new()
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
            var stream = Container.RandomStream;

            // Act
            var metadata = await Database.FindMetadataAsync(stream.Id);

            // Assert
            metadata.Should().NotBeNull();
            metadata!.Should().NotBeEmpty();
            metadata.Should().HaveSameCount(stream.Events);
            metadata!.Select(x => x.Id).Should().BeEquivalentTo(stream.Events.Select(x => x.Id));
        }
    }
}
