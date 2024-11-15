using System.Linq;

namespace StreamStore.Sql
{
    public static class EventEntityExtension
    {
        public static EventRecord ToRecord(this EventEntity entity)
        {
            return new EventRecord
            {
                Id = entity.Id!,
                Revision = entity.Revision,
                Timestamp = entity.Timestamp,
                Data = entity.Data
            };
        }

        public static EventRecord[] ToRecords(this EventEntity[] entity)
        {
            return entity.Select(ToRecord).ToArray();
        }
    }
}
