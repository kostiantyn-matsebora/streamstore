using Moq;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Lock.Composite.LockHandle
{
    public class Disposing: Scenario<S3CompositeLockHandleTestSuite>
    {


        [Fact]
        public async Task When_disposing_handle()
        {
            // Arrange
            var handle = Suite.CreateHandle();

            // Act
            await handle.DisposeAsync();

            // Assert
            Suite.FirstHandle.Verify(m => m.DisposeAsync(), Times.Once);
            Suite.SecondHandle.Verify(m => m.DisposeAsync(), Times.Once);
        }
    }
}
