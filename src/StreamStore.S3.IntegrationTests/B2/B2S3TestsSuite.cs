using Microsoft.Extensions.Configuration;
using StreamStore.S3.B2;
using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;
using StreamStore.S3.Lock;


namespace StreamStore.S3.IntegrationTests.B2
{
    class B2S3TestsSuite : IS3Suite
    {
        public bool IsReady { get; private set; }

        B2StreamDatabaseSettings? settings;

        public B2S3TestsSuite()
        {
        }

        public void Initialize()
        {
            settings = ConfigureSettings();
            IsReady = settings != null;
        }

        public IS3LockFactory? CreateLockFactory()
        {
            return CreateFactory();
        }

        public IS3ClientFactory? CreateClientFactory()
        {
            return CreateFactory();
        }

        public async Task WithDatabase(Func<IStreamDatabase, Task> action)
        {
            var database = CreateDatabase();
            await action(database!);
        }

        S3StreamDatabase? CreateDatabase()
        {
            var factory = CreateFactory();
            if (factory == null)
                return null;

            return new S3StreamDatabase(factory, factory);
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


        B2S3Factory? CreateFactory()
        {
            var storage = CreateLockStorage(settings!);

            return new B2S3Factory(settings!, new BackblazeClientFactory());
        }
    }
}
