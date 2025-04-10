using System.Linq;
using StreamStore.Storage;

namespace StreamStore.Sql
{
    public static class EventEntityExtension
    {
        public static StreamEventRecord ToRecord(this EventEntity entity)
        {
            return new StreamEventRecord
            {
                Id = entity.Id!,
                Revision = entity.Revision,
                Timestamp = entity.Timestamp,
                Data = entity.Data!
            };
        }

        public static StreamEventRecord[] ToRecords(this EventEntity[] entity)
        {
            return entity.Select(ToRecord).ToArray();
        }
    }
}
