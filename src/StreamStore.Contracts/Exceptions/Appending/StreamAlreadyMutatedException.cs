namespace StreamStore.Exceptions
{
    public sealed class StreamAlreadyMutatedException : PessimisticConcurrencyException
    {
        public Revision? ExpectedRevision { get; set; }
        public Revision? ActualRevision { get; set; }

        public StreamAlreadyMutatedException(Revision expectedRevision, Revision actualRevision, Id streamId) :
            base(streamId, $"Stream has been already changed, your revision {expectedRevision} is stale, actual revision is {actualRevision}.")
        {
            ExpectedRevision = expectedRevision;
            ActualRevision = actualRevision;
        }
    }
}