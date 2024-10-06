
namespace StreamStore.S3
{
    internal static class S3Naming
    {
        public const string Delimiter = "/";

        public static string EventKey(Id streamId, Id eventId)
            => $"streams{Delimiter}{streamId}{Delimiter}{eventId}";

        public static string StreamKey(string streamId)
            => $"streams{Delimiter}{streamId}{Delimiter}__metadata";

        public static string LockKey(Id streamId)
          => $"locks{Delimiter}{streamId}";
    }
}
