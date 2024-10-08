
using StreamStore.S3.Models;

namespace StreamStore.S3
{
    internal static class EventMetadataRecordExtension
    {
        public static S3EventMetadata ToMetadata(this EventMetadataRecord record)
        {
            return new S3EventMetadata(record);
          
        }
    }
}
