using System;
using System.Collections.Generic;

namespace StreamStore.Storage
{
    public interface IStreamEventRecordBuilder
    {
        IStreamEventRecordBuilder Dated(DateTime timestamp);
        IStreamEventRecordBuilder WithData(byte[] data);
        IStreamEventRecordBuilder WithId(Id id);
        IStreamEventRecordBuilder WithRevision(Revision revision);
        IStreamEventRecordBuilder WithRecord(IStreamEventRecord record);

        IStreamEventRecordBuilder WithCustomProperties(IEnumerable<KeyValuePair<string, string>> keyValuePairs);
    }
}