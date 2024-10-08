using System;


namespace StreamStore.Exceptions
{
    public abstract class ConcurrencyException : Exception
    {
        protected ConcurrencyException(string message) : base(message)
        {
        }
    }
}
