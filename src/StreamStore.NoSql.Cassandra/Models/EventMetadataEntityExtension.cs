using System.Linq;

namespace StreamStore.NoSql.Cassandra.Models
{
    public static class EventMetadataEntityExtension
    {
        internal static EventRecord ToRecord(this EventMetadataEntity entity)
        {
            return new EventRecord
            {
                Id = entity.Id,
                Revision = entity.Revision,
                Timestamp = entity.Timestamp,
            };
        }

        internal static EventRecord[] ToRecords(this EventMetadataEntity[] entity)
        {
            return entity.Select(ToRecord).ToArray();
        }
    }
}
