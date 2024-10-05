
namespace StreamStore
{
    public interface IStreamMetadata
    {
        Id Id { get; }
        int Revision { get; }

    }
}
