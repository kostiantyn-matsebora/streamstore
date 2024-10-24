using FluentAssertions;
using StreamStore.Exceptions;

namespace StreamStore.Testing.StreamStore.Scenarios
{
    public abstract class Reading_from_stream<TSuite> : StreamStoreScenario<TSuite> where TSuite : StreamStoreSuiteBase, new()
    {
        protected Reading_from_stream(TSuite suite) : base(suite)
        {
        }

        [SkippableFact]
        public async Task When_revision_is_greater_than_current_version_of_stream()
        {
            TrySkip();

            var stream = Container.GetExistingStream();

            Func<Task> act = async () => await Store.BeginReadAsync(stream.Id, int.MaxValue, CancellationToken.None);
            await act.Should().ThrowAsync<StreamNotFoundException>();
        }

    }
}
