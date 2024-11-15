namespace StreamStore.Tests.EventEnumerable
{
    public class ProduceConsume: Enumerating_events
    {
        public ProduceConsume() : base(StreamReadingMode.ProduceConsume)
        { }
    }
}
