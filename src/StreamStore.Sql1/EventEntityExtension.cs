using System.Linq;

namespace StreamStore.SQL
{
    public static class EventEntityExtension
    {
        public static EventRecord ToRecord(this EventEntity entity)
        {
            return new EventRecord
            {
                Id = entity.Id,
                Revision = entity.Revision,
                Timestamp = entity.Timestamp,
                Data = entity.Data
            };
        }

        public static EventRecordCollection ToCollection(this EventEntity[] entity)
        {
            return new EventRecordCollection(entity.Select(ToRecord).ToArray());
        }
    }
}
