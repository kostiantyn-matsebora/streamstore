namespace StreamStore.Testing.Models
{
    public sealed class StreamItem
    {
        public Id Id { get; }
        public EventItem[] Events { get; }
        public StreamItem(Id id, EventItem[] events)
        {
            Id = id.ThrowIfHasNoValue(nameof(id));
            Events = events ?? throw new ArgumentNullException(nameof(events));
        }

        public int Length => Events.Length;

        public int NextRevision => Length + 1;
    }
}
