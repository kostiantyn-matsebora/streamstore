using System.Linq;
using StreamStore.Storage;


namespace StreamStore.Sql
{
    public static class EventRecordCollectionExtension
    {
        public static EventEntity ToEntity(this IStreamEventRecord record, Id streamId)
        {
            return new EventEntity
            {
                Id = record.Id,
                StreamId = streamId,
                Revision = record.Revision,
                Timestamp = record.Timestamp,
                Data = record.Data!
            };
        }

        public static EventEntity[] ToEntityArray(this StreamEventRecordCollection collection, Id streamId)
        {
          return collection.Select(r => r.ToEntity(streamId)).ToArray();
        }
    }
}
