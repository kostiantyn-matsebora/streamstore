namespace StreamStore
{
    class EventConverter
    {
        readonly IEventSerializer serializer;

        public EventConverter(IEventSerializer serializer)
        {
            this.serializer = serializer;
        }

        public StreamEvent ConvertToEvent(IStreamEventRecord record)
        {
            var data = serializer.Deserialize(record.Data!);
            return new StreamEvent(record.Id, record.Revision, record.Timestamp, data);
        }

        public byte[] ConvertToByteArray(object @event)
        {
            return serializer.Serialize(@event);
        }
    }
}
