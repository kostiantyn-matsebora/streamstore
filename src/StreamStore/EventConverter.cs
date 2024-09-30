using System.Linq;


namespace StreamStore
{
    class EventConverter
    {
        readonly IEventSerializer serializer;

        public EventConverter(IEventSerializer serializer)
        {
            this.serializer = serializer;
        }

        public StreamEntity ConvertToEntity(StreamRecord record)
        {
            var eventEntities = record.Events.Select(ConvertToEntity);
            return new StreamEntity(record.Id, eventEntities);
        }

        public EventEntity ConvertToEntity(EventRecord record)
        {
            var data = serializer.Deserialize(record.Data!);
            return new EventEntity(record.Id, record.Revision, record.Timestamp, data);
        }

        public string ConvertToString(object @event)
        {
            return serializer.Serialize(@event);
        }
    }
}
