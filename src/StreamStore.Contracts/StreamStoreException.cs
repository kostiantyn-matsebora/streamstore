using System;


namespace StreamStore
{
    public abstract class StreamStoreException : Exception
    {
        protected StreamStoreException(string message) : base(message)
        {
        }
    }
}
