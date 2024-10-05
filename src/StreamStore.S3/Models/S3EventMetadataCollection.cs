
using System.Collections.Generic;
using StreamStore.S3.Client;

namespace StreamStore.S3.Models
{
    internal sealed class S3EventMetadataCollection : IS3ReadonlyMetadataCollection
    {
        readonly IS3ReadonlyMetadataCollection collection;
        const string EventId = " x-amz-meta-event-id";
        const string EventRevision = "x-amz-meta-event-revision";

        public S3EventMetadataCollection(IS3ReadonlyMetadataCollection collection)
        {
            this.collection = collection ?? throw new System.ArgumentNullException(nameof(collection));
        }

        public static S3EventMetadataCollection New(Id id, int revision)
        {
            return new S3EventMetadataCollection(
                new S3MetadataCollection()
                    .Add(EventId, id)
                    .Add(EventRevision, revision.ToString()));
        }

        public Id Id => collection[EventId];
        public int Revision => System.Convert.ToInt32(collection[EventRevision]);

        public ICollection<string> Keys => collection.Keys;

        public string this[string key] => collection[key];
    }
}
