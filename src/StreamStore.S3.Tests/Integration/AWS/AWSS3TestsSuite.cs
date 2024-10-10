using Microsoft.Extensions.Configuration;
using StreamStore.S3.AWS;
using StreamStore.S3.Lock;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Integration.AWS
{
    class AWSS3TestsSuite : ITestSuite
    {
        public static AWSS3Factory? CreateFactory()
        {
            var settings = ConfigureSettings();

            if (settings == null)
                return null;

            var storage = CreateLockStorage(settings);

            return new AWSS3Factory(settings);
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
    }
}
