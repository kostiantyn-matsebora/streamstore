using FluentAssertions;
using StreamStore.Exceptions;


namespace StreamStore.Testing.Scenarios.StreamStore
{
    public abstract class Reading_from_stream<TSuite> : StreamStoreScenario<TSuite> where TSuite : StreamStoreSuiteBase
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
