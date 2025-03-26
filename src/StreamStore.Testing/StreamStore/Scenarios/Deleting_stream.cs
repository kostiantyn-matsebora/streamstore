using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using StreamStore.Exceptions;
using StreamStore.Testing.StreamStore;


namespace StreamStore.Tests.StreamStore
{
    public abstract class Deleting_stream<TSuite> : StreamStoreScenario<TSuite> where TSuite : StreamStoreSuiteBase, new()
    {
        protected Deleting_stream(TSuite suite) : base(suite)
        {
        }

        [Fact]
        public async Task When_stream_deleted_many_times()
        {
            // Arrange
            var stream = Suite.Container.PeekStream();

            // Act
            await Suite.Store.DeleteAsync(stream.Id);

            // Assert
            var act  = async () => await Suite.Store.BeginReadAsync(stream.Id, CancellationToken.None);
            await act.Should().ThrowAsync<StreamNotFoundException>();

            // Act
            await Suite.Store.DeleteAsync(stream.Id);
            await act.Should().ThrowAsync<StreamNotFoundException>();
        }

        [Fact]
        public async Task When_stream_id_is_null()
        {
            // Act
            var act = () => Suite.Store.DeleteAsync(null!);

            // & Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
