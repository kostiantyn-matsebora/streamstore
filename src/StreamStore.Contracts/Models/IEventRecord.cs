
namespace StreamStore
{
    public interface IEventRecord: IEventMetadata
    {
        byte[] Data { get; }
    }
}
