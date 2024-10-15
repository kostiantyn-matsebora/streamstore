using System;

namespace StreamStore.Exceptions
{
    public sealed class OptimisticConcurrencyException : ConcurrencyException
    {
        public Revision? ExpectedRevision { get; set; }
        public Revision? ActualRevision { get; set; }
        public Revision? DuplicateRevision { get; set; }

        public Id StreamId { get; }


        public OptimisticConcurrencyException(Revision duplicateRevision, Id streamId) :
            base($"Stream has been already changed, revision {duplicateRevision} is already exists.")
        {
            DuplicateRevision = duplicateRevision;
            StreamId = streamId;
        }

        public OptimisticConcurrencyException(Revision expectedRevision, Revision actualRevision, Id streamId) :
            base("Stream has been already changed, your version is stale.")
        {
            ExpectedRevision = expectedRevision;
            ActualRevision = actualRevision;
            StreamId = streamId;
        }
    }
}