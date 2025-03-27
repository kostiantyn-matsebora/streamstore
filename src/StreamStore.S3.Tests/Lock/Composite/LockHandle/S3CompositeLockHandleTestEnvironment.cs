using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;


namespace StreamStore.S3.Tests.Lock.Composite.LockHandle
{
    public class S3CompositeLockHandleTestEnvironment: LockTestEnvironmentBase
    {
        public Mock<IS3LockHandle> FirstHandle { get; }
        public Mock<IS3LockHandle> SecondHandle { get; }


        public S3CompositeLockHandleTestEnvironment(): base()
        {

            FirstHandle = MockRepository.Create<IS3LockHandle>();
            FirstHandle.Setup(m => m.DisposeAsync()).Returns(new ValueTask());
            FirstHandle.Setup(m => m.ReleaseAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            SecondHandle = MockRepository.Create<IS3LockHandle>();
            SecondHandle.Setup(m => m.DisposeAsync()).Returns(new ValueTask());
            SecondHandle.Setup(m => m.ReleaseAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        }

        internal S3CompositeLockHandle CreateHandle()
        {
            return new S3CompositeLockHandle(FirstHandle.Object, SecondHandle.Object);
        }
    }
}
