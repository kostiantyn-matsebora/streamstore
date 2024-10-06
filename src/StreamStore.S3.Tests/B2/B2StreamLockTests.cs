using System.Configuration;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using StreamStore.S3.B2;
using StreamStore.S3.Client;
using StreamStore.Serialization;
using Xunit.Abstractions;


namespace StreamStore.S3.Tests.AmazonS3
{
    public class B2StreamLockTests
    {

        B2S3Factory? factory;
        ITestOutputHelper output;
        public B2StreamLockTests(ITestOutputHelper output)
        {
            this.output = output;
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

            factory = new B2S3Factory(settings);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        public void AcquireAsync_ShouldAcquireLockOnlyOnce(int parallelAttempts)
        {
            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();
            var acquirances = new List<IS3StreamLock>();

            Parallel.For(0, parallelAttempts, i =>
            {
                var @lock = factory!.CreateLock(streamId);
                if (@lock.AcquireAsync(CancellationToken.None).Result)
                    acquirances.Add(@lock);
            });
            acquirances.Count.Should().Be(1);
            acquirances.ForEach(l => l.ReleaseAsync(CancellationToken.None).Wait());
        }


        [Fact]
        public async Task AcquireAsync_ShouldNotAllowToAcquireLockIfAlreadyExists()
        {
            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();

            // Act & Assert
            var @lock = factory!.CreateLock(streamId);
            var result = await @lock.AcquireAsync(CancellationToken.None);
            result.Should().BeTrue();

            var lock2 = factory!.CreateLock(streamId);
            result = await @lock2.AcquireAsync(CancellationToken.None);
            result.Should().BeFalse();

            await @lock.ReleaseAsync(CancellationToken.None);
            await lock2.ReleaseAsync(CancellationToken.None);
        }
    }
}
