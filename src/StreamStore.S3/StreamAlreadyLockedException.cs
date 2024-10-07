using System;


namespace StreamStore.S3
{
    public sealed class StreamAlreadyLockedException: Exception
    {
        public Id StreamId { get; }
        public StreamAlreadyLockedException(Id streamId) : base($"Stream {streamId} is already locked or you don't have permissions to lock it.")
        {
            StreamId = streamId;
        }
    }
}
