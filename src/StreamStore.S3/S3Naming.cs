
namespace StreamStore.S3
{
    internal sealed class S3Naming
    {
        public const string Delimiter = "/";

        public static string EventKey(Id streamId, Id eventId)
            => $"{streamId}{Delimiter}events{Delimiter}{eventId}";

        public static string StreamKey(string streamId)
            => $"{streamId}{Delimiter}metadata";

        public static string LockKey(Id streamId)
          => $"{streamId}{Delimiter}lock";
    }
}
