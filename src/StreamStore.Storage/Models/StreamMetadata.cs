
using System;

namespace StreamStore.Storage
{
    public sealed class StreamMetadata : IStreamMetadata
    {
        public StreamMetadata(Id id, Revision revision, DateTime lastModified)
        {
            Id = id;
            Revision = revision;
            LastModified = lastModified;
        }
        public Id Id { get; }

        public Revision Revision { get; }
        public DateTime LastModified { get; }
    }
}
