using System;

namespace StreamStore
{
    [Serializable]
    public sealed class OptimisticConcurrencyException : StreamStoreException
    {
        public int ExpectedRevision { get; set; }
        public int ActualRevision { get; set; }

        public int DuplicateRevision { get; set; }

        public Id StreamId { get; }
        public OptimisticConcurrencyException(int duplicateRevision, Id streamId) :
            base($"Stream has been already changed, revision {duplicateRevision} is already exists.")
        {
            DuplicateRevision = duplicateRevision;
            StreamId = streamId;
        }

        public OptimisticConcurrencyException(int expectedRevision, int actualRevision, Id streamId) :
            base("Stream has been already changed, your version is stale.")
        {
            ExpectedRevision = expectedRevision;
            ActualRevision = actualRevision;
            StreamId = streamId;
        }
    }
}