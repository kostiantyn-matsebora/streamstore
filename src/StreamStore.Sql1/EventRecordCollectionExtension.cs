using System;
using System.Linq;


namespace StreamStore.SQL
{
    public static class EventRecordCollectionExtension
    {
        public static EventEntity ToEntity(this EventRecord record, Id streamId)
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

        public static EventEntity[] ToEntityArray(this EventRecordCollection collection, Id streamId)
        {
          return collection.Select(r => r.ToEntity(streamId)).ToArray();
        }
    }
}
