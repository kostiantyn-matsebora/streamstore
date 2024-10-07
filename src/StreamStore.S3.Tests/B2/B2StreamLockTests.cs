using System.Collections.Concurrent;
using AutoFixture;
using FluentAssertions;

using StreamStore.S3.B2;
using StreamStore.S3.Client;


namespace StreamStore.S3.Tests.B2
{
    public class B2StreamLockTests
    {

        readonly B2S3Factory? factory;

        public B2StreamLockTests()
        {
            factory = B2TestsSuite.CreateFactory();
        }

        [InlineData(1000)]
        [InlineData(100)]
        [InlineData(10)]
        [SkippableTheory]
        public async Task AcquireAsync_ShouldAcquireLockOnlyOnce(int parallelAttempts)
        {
            Skip.IfNot(factory != null, "B2 configuration is missing");

            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();
            var acquirances = new ConcurrentBag<IS3LockHandle>();

            // Act
            var tasks = Enumerable.Range(0, parallelAttempts).Select(async i =>
            {
                var @lock = factory!.CreateLock(streamId);
                var handle = await @lock.AcquireAsync(CancellationToken.None);

                if (handle != null)
                    acquirances.Add(handle);
            });

            await Task.WhenAll(tasks);

            // Assert
            acquirances.Count.Should().Be(1);

            var releaseTasks = acquirances.Select(handle => handle.ReleaseAsync(CancellationToken.None));
            await Task.WhenAll(releaseTasks);
        }


        [SkippableFact]
        public async Task AcquireAsync_ShouldNotAllowToAcquireLockIfAlreadyExists()
        {
            Skip.IfNot(factory != null, "B2 configuration is missing");

            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();

            // Act & Assert
            var @lock = factory!.CreateLock(streamId);
            var handle = await @lock.AcquireAsync(CancellationToken.None);
            handle.Should().NotBeNull();

            var lock2 = factory!.CreateLock(streamId);
            var handle2 = await lock2.AcquireAsync(CancellationToken.None);
            handle2.Should().BeNull();

            await handle!.ReleaseAsync(CancellationToken.None);
        }

        [SkippableFact]
        public async Task AcquireAsync_ShouldAcquireLockIfReleased()
        {
            Skip.IfNot(factory != null, "B2 configuration is missing");

            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();

            // Act & Assert
            var @lock = factory!.CreateLock(streamId);
            var handle = await @lock.AcquireAsync(CancellationToken.None);
            handle.Should().NotBeNull();

            (await @lock.AcquireAsync(CancellationToken.None)).Should().BeNull();

            await handle!.ReleaseAsync(CancellationToken.None);

            var lock2 = factory!.CreateLock(streamId);
            var handle2 = await lock2.AcquireAsync(CancellationToken.None);
            handle2.Should().NotBeNull();

            await handle2!.ReleaseAsync(CancellationToken.None);
        }
    }
}
