using System;


namespace StreamDB
{
    public abstract class StreamDbException: Exception
    {
        protected StreamDbException(string message) : base(message)
        {
        }
    }
}
