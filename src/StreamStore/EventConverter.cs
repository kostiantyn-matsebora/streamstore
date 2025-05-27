using StreamStore.Storage.Models;

namespace StreamStore
{
    class EventConverter: IEventConverter
    {
        readonly IEventSerializer serializer;

        public EventConverter(IEventSerializer serializer)
        {
            this.serializer = serializer;
        }

        public IStreamEventEnvelope ConvertToEnvelope(IStreamEventRecord record)
        {
            var data = serializer.Deserialize(record.Data!);
            return new StreamEventEnvelope(
                record.Id, 
                record.Revision, 
                record.Timestamp, 
                data, 
                new EventCustomProperties(record.CustomProperties));
        }

        public byte[] ConvertToByteArray(object @event)
        {
            return serializer.Serialize(@event);
        }
    }
}
