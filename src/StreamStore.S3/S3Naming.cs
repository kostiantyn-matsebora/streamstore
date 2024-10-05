
namespace StreamStore.S3
{
    internal sealed class S3Naming
    {
        public const string Delimiter = "/";

        public static string EventKey(Id streamId, Id eventId)
            => $"stream_{streamId}{Delimiter}event_{eventId}";

        public static string StreamEventPrefix(Id streamId)
            => $"stream_{streamId}{Delimiter}event_";

        public static string StreamKey(string streamId)
            => $"stream_{streamId}{Delimiter}/stream";
    }
}
