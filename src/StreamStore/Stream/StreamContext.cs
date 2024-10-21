using System;



namespace StreamStore
{
    public class StreamContext
    {
        public Id StreamId { get; }
        public StreamMetadataRecord Metadata { get; }
        public int PageSize { get; }

        public Revision CurrentRevision => Metadata.Revision;

        public StreamContext(Id streamId, StreamMetadataRecord metadata, int pageSize = 10)
        {
            StreamId = streamId.ThrowIfHasNoValue(nameof(streamId));
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            PageSize = pageSize;
        }
    }
}
