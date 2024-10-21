using FluentAssertions;
using StreamStore.Exceptions;


namespace StreamStore.Tests.Scenarios
{
    public class ReadFromStream : Scenario
    {
        [Fact]
        public async Task When_revision_is_greater_than_current_version_of_stream()
        {
            Func<Task> act = async () =>  await store.BeginReadAsync(RandomStreamId, CancellationToken.None); 
            await act.Should().ThrowAsync<StreamNotFoundException>();
        }
    }
}
