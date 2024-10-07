using Microsoft.Extensions.Configuration;
using StreamStore.S3.B2;
using StreamStore.S3.Lock;

namespace StreamStore.S3.Tests.B2
{
    class B2TestsSuite
    {
        public static B2S3Factory? CreateFactory()
        {
            if (!File.Exists(
                    Path.Combine(AppContext.BaseDirectory, "appsettings.Development.json")))
                return null;

            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile($"appsettings.Development.json", true)
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

            var storage = new S3InMemoryStreamLockStorage(settings.InMemoryLockTTL);

            return new B2S3Factory(settings, storage);
        }
    }
}
