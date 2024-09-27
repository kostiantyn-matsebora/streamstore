using System;


namespace StreamStore
{
    public abstract class StreamDbException : Exception
    {
        protected StreamDbException(string message) : base(message)
        {
        }
    }
}
