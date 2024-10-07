
namespace StreamStore.S3
{
    internal static class S3Naming
    {
        public const string Delimiter = "/";

        public static string LockPrefix => $"locks{Delimiter}";
        
        public static string StreamPrefix(Id streamId) => $"streams{Delimiter}{streamId}{Delimiter}";

        public static string EventKey(Id streamId, Id eventId) => $"{StreamPrefix(streamId)}{eventId}";

        public static string StreamMetadataKey(string streamId) => $"{StreamPrefix(streamId)}__metadata";

        public static string LockKey(Id streamId) => $"{LockPrefix}{streamId}";
    }
}
