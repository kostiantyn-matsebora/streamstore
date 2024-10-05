

namespace StreamStore
{
    public sealed class StreamMetadata : IStreamMetadata
    {
        public Id Id { get; }
        public int Revision { get; }

        public StreamMetadata(Id id, int revision)
        {
            Id = id;
            Revision = revision;
        }
    }
}
