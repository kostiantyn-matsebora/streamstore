using StreamStore.S3.AWS;
using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;
using StreamStore.S3.Lock;
using StreamStore.S3.Storage;
using StreamStore.Testing;

namespace StreamStore.S3.IntegrationTests.AWS
{
    class AWSS3TestsSuite : IS3Suite
    {
        public bool IsReady { get; private set;}

        AWSS3DatabaseSettings? settings;

        public AWSS3TestsSuite()
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

        public IStreamUnitOfWork? CreateUnitOfWork(Id streamId, Revision expectedRevision)
        {
            var factory = CreateFactory();
            if (factory == null)
                return null;

            return new S3StreamUnitOfWork(factory, new S3StreamContext(streamId, expectedRevision, new S3Storage(factory)));
        }

        public async Task WithDatabase(Func<IStreamDatabase, Task> action)
        {
            var database = CreateDatabase();
            await action(database!);
        }

        static AWSS3DatabaseSettings? ConfigureSettings()
        {

            if (!File.Exists(
                    Path.Combine(AppContext.BaseDirectory, "appsettings.Development.json")))
                return null;

            return
                 new AWSS3DatabaseSettingsBuilder()
             .Build();
        }

        static S3InMemoryStreamLockStorage CreateLockStorage(AWSS3DatabaseSettings settings)
        {
            return new S3InMemoryStreamLockStorage(settings.InMemoryLockTTL);
        }


        AWSS3Factory? CreateFactory()
        {
            var storage = CreateLockStorage(settings!);
            return new AWSS3Factory(settings!, new AmazonS3ClientFactory());
        }

        IStreamDatabase? CreateDatabase()
        {
            var factory = CreateFactory();

            return new S3StreamDatabase(factory!, factory!);
        }
    }
}
