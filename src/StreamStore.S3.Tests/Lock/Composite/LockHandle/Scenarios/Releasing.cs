using Moq;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Lock.Composite.LockHandle
{
    public class Releasing: Scenario<S3CompositeLockHandleTestSuite>
    {

        [Fact]
        public async Task When_releasing()
        {
            // Arrange
            var handle = Suite.CreateHandle();
            CancellationToken token = default(global::System.Threading.CancellationToken);

            // Act
            await handle.ReleaseAsync(
                token);

            // Assert
            Suite.FirstHandle.Verify(m => m.ReleaseAsync(It.IsAny<CancellationToken>()), Times.Once);
            Suite.SecondHandle.Verify(m => m.ReleaseAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
