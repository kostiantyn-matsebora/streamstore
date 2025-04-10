using System.Linq;
using StreamStore.Storage;

namespace StreamStore.NoSql.Cassandra.Models
{
    public static class EventEntityExtension
    {
        internal static IStreamEventRecord ToRecord(this EventEntity entity)
        {
            return new StreamEventRecord
            {
                Id = entity.Id,
                Revision = entity.Revision,
                Timestamp = entity.Timestamp,
                Data = entity.Data!
            };
        }

        internal static IStreamEventRecord[] ToRecords(this EventEntity[] entity)
        {
            return entity.Select(ToRecord).ToArray();
        }
    }
}
