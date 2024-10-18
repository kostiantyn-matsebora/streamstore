namespace StreamStore.Testing
{
    public class EventItem
    {
        public Id Id { get; set; }

        public DateTime Timestamp { get; set; }

        public byte[]? Data { get; set; }
    }
}
