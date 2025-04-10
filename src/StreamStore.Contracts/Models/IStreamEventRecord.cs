
namespace StreamStore
{
    public interface IStreamEventRecord: IStreamEventMetadata
    {
        public byte[] Data { get; }
    }
}
