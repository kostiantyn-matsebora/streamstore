using System.Linq;


namespace StreamStore.Domain
{
    internal class Converter
    {
        readonly IEventSerializer serializer;

        public Converter(IEventSerializer serializer)
        {
            this.serializer = serializer;
        }

        public StreamEntity ToEntity(StreamRecord record)
        {
            var eventEntities = record.Events.Select(ToEntity);
            return new StreamEntity(record.Id, eventEntities);
        }

        public EventEntity ToEntity(EventRecord record)
        {
            var data = serializer.Deserialize(record.Data!);
            return new EventEntity(record.Id, record.Revision, record.Timestamp, data);
        }
    }
}
