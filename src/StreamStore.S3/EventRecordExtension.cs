using StreamStore.S3.Models;

namespace StreamStore.S3
{
    internal static class EventRecordExtension
    {
        public static S3EventMetadata ToMetadata(this EventRecord record)
        {
            return new S3EventMetadata(record);
        }
    }
}
