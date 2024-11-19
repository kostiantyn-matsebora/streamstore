namespace StreamStore.NoSql.Cassandra.Models
{
    public static class EventRecordExtension
    {
        internal static EventEntity ToEntity(this EventRecord record, Id streamId)
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
