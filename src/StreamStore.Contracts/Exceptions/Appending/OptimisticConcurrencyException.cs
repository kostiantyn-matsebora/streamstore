using System;

namespace StreamStore.Exceptions
{
    public class OptimisticConcurrencyException : ConcurrencyControlException
    {

        public OptimisticConcurrencyException(Id streamId, string message) : base(streamId, message)
        {
        }

    }
}