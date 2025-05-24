using System;

namespace StreamStore.Exceptions
{
    public class OptimisticConcurrencyException : ConcurrencyException
    {

        public OptimisticConcurrencyException(Id streamId, string message) : base(streamId, message)
        {
        }

    }
}