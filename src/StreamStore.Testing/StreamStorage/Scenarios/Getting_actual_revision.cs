using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;

namespace StreamStore.Testing.StreamStorage.Scenarios
{
    public abstract class Get_actual_revision<TEnvironment> : StorageScenario<TEnvironment> where TEnvironment : StorageTestEnvironmentBase, new()
    {
        protected Get_actual_revision(TEnvironment environment) : base(environment)
        {
        }


        [SkippableFact]
        public async Task When_stream_does_not_exist()
        {
            TrySkip();

            var revision = await Storage.GetActualRevision(Generated.Primitives.Id);
            revision.Should().BeNull();
        }


        [SkippableFact]
        public async Task When_stream_does_exist()
        {
            TrySkip();

            // Arrange
            var stream = Container.RandomStream;

            // Act
            var revision = await Storage.GetActualRevision(stream.Id);

            // Assert
            revision.Should().NotBeNull();
            revision.Should().Be(stream.Events.Count());
        }
    }
}
