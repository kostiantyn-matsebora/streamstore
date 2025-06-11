namespace StreamStore.Tests.EventEnumerable
{
    public class Default: Enumerating_events
    {
        public Default(): base(StreamReadingMode.Queue)
        {
        }
    }
}
