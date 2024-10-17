using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace StreamStore
{
    class EventConverter
    {
        readonly IEventSerializer serializer;

        public EventConverter(IEventSerializer serializer)
        {
            this.serializer = serializer;
        }

        public StreamEntity ConvertToEntity(Id id, StreamRecord record)
        {
            var eventEntities = record.Events.AsParallel().Select(ConvertToEntity).ToArray();
            return new StreamEntity(id, eventEntities);
        }

        public EventEntity ConvertToEntity(EventRecord record)
        {
            var data = serializer.Deserialize(record.Data!);
            return new EventEntity(record.Id, record.Revision, record.Timestamp, data);
        }

        public byte[] ConvertToByteArray(object @event)
        {
            return serializer.Serialize(@event);
        }
    }
}
