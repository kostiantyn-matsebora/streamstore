using StreamStore.S3.Client;

namespace StreamStore.S3.IntegrationTests
{
    internal interface IS3Suite
    {
        IS3LockFactory? CreateFactory();
        IStreamUnitOfWork CreateUnitOfWork();
        IStreamDatabase CreateDatabase();
    }
}
