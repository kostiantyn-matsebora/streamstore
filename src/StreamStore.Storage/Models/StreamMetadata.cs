
namespace StreamStore.Storage
{
    public sealed class StreamMetadata : IStreamMetadata
    {
        public StreamMetadata(Id id, Revision revision)
        {
            Id = id;
            Revision = revision;
          
        }
        public Id Id { get; }

        public Revision Revision { get; }
    }
}
