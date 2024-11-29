using System.Linq;

namespace StreamStore.NoSql.Cassandra.Models
{
    public static class EventEntityExtension
    {
        internal static EventRecord ToRecord(this EventEntity entity)
        {
            return new EventRecord
            {
                Id = entity.Id,
                Revision = entity.Revision,
                Timestamp = entity.Timestamp,
                Data = entity.Data
            };
        }

        internal static EventRecord[] ToRecords(this EventEntity[] entity)
        {
            return entity.Select(ToRecord).ToArray();
        }
    }
}
