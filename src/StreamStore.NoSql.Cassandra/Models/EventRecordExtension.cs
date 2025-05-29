using System.Collections.Generic;

using StreamStore.Extensions;

namespace StreamStore.NoSql.Cassandra.Models
{
    public static class EventRecordExtension
    {
        internal static EventEntity ToEntity(this IStreamEventRecord record, Id streamId)
        {
            return new EventEntity
            {
                Id = record.Id,
                StreamId = streamId,
                Timestamp = record.Timestamp,
                Revision = record.Revision,
                Data = record.Data,
                CustomProperties = record.CustomProperties.NotNullAndNotEmpty() ? new Dictionary<string,string>(record.CustomProperties) : null!
            };
        }
    }
}
