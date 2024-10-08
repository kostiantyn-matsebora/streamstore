using System;


namespace StreamStore.Exceptions
{
    public abstract class ConcurrencyException : Exception
    {
        public ConcurrencyException(string message) : base(message)
        {
        }
    }
}
