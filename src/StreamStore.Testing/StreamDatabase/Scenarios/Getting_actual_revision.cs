using FluentAssertions;

namespace StreamStore.Testing.StreamDatabase.Scenarios
{
    public abstract class Get_actual_revision<TSuite> : DatabaseScenario<TSuite> where TSuite : DatabaseSuiteBase, new()
    {
        protected Get_actual_revision(TSuite suite) : base(suite)
        {
        }


        [SkippableFact]
        public async Task When_stream_does_not_exist()
        {
            TrySkip();

            var revision = await Database.GetActualRevision(Generated.Id);
            revision.Should().BeNull();
        }


        [SkippableFact]
        public async Task When_stream_does_exist()
        {
            TrySkip();

            // Arrange
            var stream = Container.RandomStream;

            // Act
            var revision = await Database.GetActualRevision(stream.Id);

            // Assert
            revision.Should().NotBeNull();
            revision.Should().Be(stream.Events.Count());
        }
    }
}
