using Moq;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Lock.Composite.LockHandle
{
    public class Releasing: Scenario<S3CompositeLockHandleTestEnvironment>
    {

        [Fact]
        public async Task When_releasing()
        {
            // Arrange
            var handle = Environment.CreateHandle();
            CancellationToken token = default(global::System.Threading.CancellationToken);

            // Act
            await handle.ReleaseAsync(
                token);

            // Assert
            Environment.FirstHandle.Verify(m => m.ReleaseAsync(It.IsAny<CancellationToken>()), Times.Once);
            Environment.SecondHandle.Verify(m => m.ReleaseAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
