using System.Collections.Concurrent;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using StreamStore.S3.B2;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;
using Xunit.Abstractions;


namespace StreamStore.S3.Tests.B2
{
    public class B2StreamLockTests
    {

        readonly B2S3Factory? factory;

        public B2StreamLockTests()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile($"appsettings.Development.json")
                .Build();

            var b2Section = config.GetSection("b2");

            var settings =
                 new B2StreamDatabaseSettingsBuilder()
                 .WithCredentials(
                     b2Section.GetSection("applicationKeyId").Value!,
                     b2Section.GetSection("applicationKey").Value!)
                 .WithBucketId(b2Section.GetSection("bucketId").Value!)
                 .WithBucketName(b2Section.GetSection("bucketName").Value!)
             .Build();

            var storage = new S3StreamLockStorage();

            factory = new B2S3Factory(settings, storage);
        }

        //[Theory]
        //[InlineData(1000)]
        //[InlineData(100)]
        //[InlineData(10)]
        public async Task AcquireAsync_ShouldAcquireLockOnlyOnce(int parallelAttempts)
        {
            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();
            var acquirances = new ConcurrentBag<IS3LockHandle>();

            var tasks = Enumerable.Range(0, parallelAttempts).Select(async i =>
            {
                var @lock = factory!.CreateLock(streamId);
                var handle = await @lock.AcquireAsync(CancellationToken.None);

                if (handle != null)
                    acquirances.Add(handle);
            });

            await Task.WhenAll(tasks);

            acquirances.Count.Should().Be(1);

            var releaseTasks = acquirances.Select(handle => handle.ReleaseAsync(CancellationToken.None));
            await Task.WhenAll(releaseTasks);
        }


        // [Fact]
        public async Task AcquireAsync_ShouldNotAllowToAcquireLockIfAlreadyExists()
        {
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
    }
}
