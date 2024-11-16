using System;
using System.Collections.Generic;
using System.Text;

namespace StreamStore.NoSql.Cassandra.Models
{
    public static class EventRecordExtension
    {
        public static EventEntity ToEntity(this EventRecord record, Id streamId)
        {
            return new EventEntity
            {
                Id = record.Id,
                StreamId = streamId,
                Timestamp = record.Timestamp,
                Revision = record.Revision,
                Data = record.Data
            };
        }
    }
}
