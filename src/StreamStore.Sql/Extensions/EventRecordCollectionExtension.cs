using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using StreamStore.Extensions;


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
                Data = record.Data!,
                CustomProperties = record.CustomProperties.NotNullAndNotEmpty() ? JsonSerializer.Serialize(record.CustomProperties!): null
            };
        }

        public static EventEntity[] ToEntityArray(this IEnumerable<IStreamEventRecord> collection, Id streamId)
        {
          return collection.Select(r => r.ToEntity(streamId)).ToArray();
        }
    }
}
