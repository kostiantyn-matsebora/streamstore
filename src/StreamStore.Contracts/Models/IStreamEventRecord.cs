
using StreamStore.Models;

namespace StreamStore
{
    public interface IStreamEventRecord : IStreamEventMetadata
    {
        byte[] Data { get; }

        ICustomProperties CustomProperties { get; }
    }
}
