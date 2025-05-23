namespace StreamStore
{
    public interface IStreamMetadata
    {
        Id Id { get; }
        Revision Revision { get; }
    }
}
