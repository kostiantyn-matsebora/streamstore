using Microsoft.Extensions.Configuration;
using StreamStore.S3.B2;
using StreamStore.S3.Lock;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.B2
{
    class B2S3TestsSuite: ITestSuite
    {
        public static B2S3Factory? CreateFactory()
        {
            var settings = ConfigureSettings();

            if (settings == null)
                return null;

            var storage = CreateLockStorage(settings);

            return new B2S3Factory(settings, storage);
        }

        public static IStreamUnitOfWork? CreateUnitOfWork(Id streamId, int expectedRevision = 0)
        {
            var factory = CreateFactory();
            if (factory == null)
                return null;

            return new S3StreamUnitOfWork(streamId, expectedRevision, factory);
        }

        public IStreamDatabase? CreateDatabase()
        {
           var factory = CreateFactory();
            if (factory == null)
                return null;

            return new S3StreamDatabase(factory);
        }

        static B2StreamDatabaseSettings? ConfigureSettings()
        {

            if (!File.Exists(
                    Path.Combine(AppContext.BaseDirectory, "appsettings.Development.json")))
                return null;

            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile($"appsettings.Development.json", true)
                .Build();

            var b2Section = config.GetSection("b2");

            return
                 new B2StreamDatabaseSettingsBuilder()
                 .WithCredentials(
                     b2Section.GetSection("applicationKeyId").Value!,
                     b2Section.GetSection("applicationKey").Value!)
                 .WithBucketId(b2Section.GetSection("bucketId").Value!)
                 .WithBucketName(b2Section.GetSection("bucketName").Value!)
             .Build();
        }

        static S3InMemoryStreamLockStorage CreateLockStorage(B2StreamDatabaseSettings settings)
        {
            return new S3InMemoryStreamLockStorage(settings.InMemoryLockTTL);
        }
    }
}
