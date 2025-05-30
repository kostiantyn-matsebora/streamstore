using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using StreamStore.Exceptions.Reading;
using StreamStore.Testing.StreamStore;


namespace StreamStore.Tests.StreamStore.Scenarios
{
    public abstract class Deleting_stream<TEnvironment> : StreamStoreScenario<TEnvironment> where TEnvironment : StreamStoreTestEnvironmentBase, new()
    {
        protected Deleting_stream(TEnvironment environment) : base(environment)
        {
        }

        [Fact]
        public async Task When_stream_deleted_many_times()
        {
            // Arrange
            var stream = Environment.Container.PeekStream();

            // Act
            await Environment.Store.DeleteAsync(stream.Id);

            // Assert
            var act  = async () => await Environment.Store.BeginReadAsync(stream.Id, CancellationToken.None);
            await act.Should().ThrowAsync<StreamNotFoundException>();

            // Act
            await Environment.Store.DeleteAsync(stream.Id);
            await act.Should().ThrowAsync<StreamNotFoundException>();
        }

        [Fact]
        public async Task When_stream_id_is_null()
        {
            // Act
            var act = () => Environment.Store.DeleteAsync(null!);

            // & Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
