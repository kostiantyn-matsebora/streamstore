using System;
using System.Collections.Generic;
using Amazon.S3.Model;

namespace StreamStore.S3.AmazonS3
{
    internal sealed class AmazonStreamMetadata : IStreamMetadata
    {
        readonly MetadataCollection collection;

        const string StreamId = " x-amz-meta-stream-id";
        const string StreamRevision = "x-amz-meta-stream-revision";

        public string this[string key] => collection[key];

        public ICollection<string> Keys => collection.Keys;

        public Id Id => collection[StreamId];

        public int Revision => Convert.ToInt32(collection[StreamRevision]);

        public AmazonStreamMetadata(MetadataCollection metadata)
        {
            this.collection = metadata ?? throw new ArgumentNullException(nameof(metadata));
        }

        public AmazonStreamMetadata(IStreamMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException(nameof(metadata));
            this.collection = new MetadataCollection();
            collection[StreamId] = metadata.Id;
            collection[StreamRevision] = metadata.Revision.ToString();
        }
    }
}
