using System;

namespace StreamStore.Exceptions.Appending
{
    public class OptimisticConcurrencyException : ConcurrencyControlException
    {

        public OptimisticConcurrencyException(Id streamId, string message) : base(streamId, message)
        {
        }

    }
}