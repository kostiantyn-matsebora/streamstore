using EventFlow.EventStores;

namespace StreamStore.Storage.EventFlow
{
    internal class CommittedDomainEvent : ICommittedDomainEvent
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string AggregateId { get; set; }


        public string Data { get; set; }

        public string Metadata { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public int AggregateSequenceNumber { get; set; }
    }
}