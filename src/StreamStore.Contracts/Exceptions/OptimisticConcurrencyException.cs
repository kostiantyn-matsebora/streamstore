namespace StreamStore.Exceptions
{
    public sealed class OptimisticConcurrencyException : ConcurrencyException
    {
        public Revision? ExpectedRevision { get; set; }
        public Revision? ActualRevision { get; set; }


        public OptimisticConcurrencyException(Revision expectedRevision, Revision actualRevision, Id streamId) :
            base(streamId, "Stream has been already changed, your version is stale.")
        {
            ExpectedRevision = expectedRevision;
            ActualRevision = actualRevision;
        }
    }
}