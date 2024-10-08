using System;


namespace StreamStore.S3
{
    public sealed class S3StreamAlreadyLockedException: Exception
    {
        public Id StreamId { get; }
        public S3StreamAlreadyLockedException(Id streamId) : base($"Stream {streamId} is already locked or you don't have permissions to lock it.")
        {
            StreamId = streamId;
        }
    }
}
