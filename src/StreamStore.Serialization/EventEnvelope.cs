namespace StreamStore.Serialization
{
    public sealed class EventEnvelope: IEventEnvelope
    {
        public string? Type { get; set; }
        public byte[]? Data { get; set; }
    }
}
