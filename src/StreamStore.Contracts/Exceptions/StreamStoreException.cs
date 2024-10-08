using System;


namespace StreamStore.Exceptions
{
    public abstract class StreamStoreException : Exception
    {
        protected StreamStoreException(string message) : base(message)
        {
        }
    }
}
