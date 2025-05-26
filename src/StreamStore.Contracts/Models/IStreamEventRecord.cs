
namespace StreamStore
{
    public interface IStreamEventRecord : IStreamEventMetadata
    {
        byte[] Data { get; }
    }
}
