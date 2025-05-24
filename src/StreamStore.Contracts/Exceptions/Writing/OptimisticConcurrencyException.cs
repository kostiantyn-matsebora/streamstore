using System;

namespace StreamStore.Exceptions
{
    public class OptimisticConcurrencyException : ConcurrencyException
    {

        public OptimisticConcurrencyException(Id streamId, string message) : base(streamId, message)
        {
        }

    }

    public class StreamAlreadyChangedException : OptimisticConcurrencyException
    {
        public Revision? ExpectedRevision { get; set; }
        public Revision? ActualRevision { get; set; }

        public StreamAlreadyChangedException(Revision expectedRevision, Revision actualRevision, Id streamId) :
            base(streamId, $"Stream has been already changed, your revision {expectedRevision} is stale, actual revision is {actualRevision}.")
        {
            ExpectedRevision = expectedRevision;
            ActualRevision = actualRevision;
        }
    }
}