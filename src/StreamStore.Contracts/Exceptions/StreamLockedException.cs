namespace StreamStore.Exceptions
{
    public class StreamLockedException: ConcurrencyException
    {
        public StreamLockedException(Id streamId) : base(streamId, $"Stream {streamId} is being changed right now.")
        {
        }
    }
}
