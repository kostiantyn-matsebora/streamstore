using System;


namespace StreamStore.Exceptions
{
    public abstract class StreamStoreException : Exception
    {
        public Id StreamId { get; }

        protected StreamStoreException(Id streamId, string message) : base(message)
        {
            StreamId = streamId;
        }
    }
}
