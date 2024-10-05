using System;


namespace StreamStore.S3
{
    public class StreamLockedException: Exception
    {
        public Id StreamId { get; }
        public StreamLockedException(Id streamId) : base($"Stream {streamId} is locked.")
        {
            StreamId = streamId;
        }
    }
}
