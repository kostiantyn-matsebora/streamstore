using System;
namespace StreamStore
{
    public class EventMetadataRecord: IHasRevision
    {
        public Id Id { get; set; }

        public DateTime Timestamp { get; set; }

        public Revision Revision { get; set; }
    }
}
