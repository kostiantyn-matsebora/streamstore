using System.Collections.Generic;
using System.Linq;


namespace StreamStore
{
    public static class IStreamEventRecordExtension
    {
        public static Revision MaxRevision(this IEnumerable<IStreamEventRecord> records)
        {
            if (records == null || !records.Any()) return Revision.Zero;
            return records.Max(e => e.Revision);
        }


        public static Revision MinRevision(this IEnumerable<IStreamEventRecord> records)
        {
            if (records == null || !records.Any()) return Revision.Zero;
            return records.Min(e => e.Revision);
        }
    }
}
