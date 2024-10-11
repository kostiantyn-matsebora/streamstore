using StreamStore.S3.B2;
using StreamStore.S3.Client;

namespace StreamStore.S3.Tests.Integration
{
    internal interface IS3Suite
    {
        IS3Factory? CreateFactory();
        IStreamUnitOfWork CreateUnitOfWork();
        IStreamDatabase CreateDatabase();
    }
}
