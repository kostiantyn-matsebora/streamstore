using Moq;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Lock.Composite.LockHandle
{
    public class Disposing: Scenario<S3CompositeLockHandleTestEnvironment>
    {


        [Fact]
        public async Task When_disposing_handle()
        {
            // Arrange
            var handle = Environment.CreateHandle();

            // Act
            await handle.DisposeAsync();

            // Assert
            Environment.FirstHandle.Verify(m => m.DisposeAsync(), Times.Once);
            Environment.SecondHandle.Verify(m => m.DisposeAsync(), Times.Once);
        }
    }
}
