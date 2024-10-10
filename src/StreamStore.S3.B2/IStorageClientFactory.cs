
using Bytewizer.Backblaze.Client;

namespace StreamStore.S3.B2
{
    public interface IStorageClientFactory
    {
        public IStorageClient Create();
    }
}
