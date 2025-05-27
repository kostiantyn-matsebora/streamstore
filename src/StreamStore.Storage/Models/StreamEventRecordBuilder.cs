using System;
using System.Collections.Generic;
using StreamStore.Storage.Models;


namespace StreamStore.Storage
{
    class StreamEventRecordBuilder : IStreamEventRecordBuilder
    {
        Id id;
        DateTime timestamp = DateTime.Now;
        byte[] data = null!;
        Revision revision;
        EventCustomProperties customProperties = EventCustomProperties.Empty();

        public IStreamEventRecordBuilder WithId(Id id)
        {
            this.id = id;
            return this;
        }

        public IStreamEventRecordBuilder Dated(DateTime timestamp)
        {
            this.timestamp = timestamp;
            return this;
        }

        public IStreamEventRecordBuilder WithData(byte[] data)
        {
            this.data = data;
            return this;
        }

        public IStreamEventRecordBuilder WithRevision(Revision revision)
        {
            this.revision = revision;
            return this;
        }

        public IStreamEventRecord Build()
        {
            id.ThrowIfHasNoValue(nameof(id));
            timestamp.ThrowIfMinValue(nameof(timestamp));
            data.ThrowIfNull(nameof(data));
            customProperties.ThrowIfNull(nameof(customProperties));

            if (revision == Revision.Zero)
                throw new ArgumentOutOfRangeException(nameof(revision));

            return new StreamEventRecord
            {
                Id = id,
                Timestamp = timestamp,
                Data = data,
                Revision = revision,
                CustomProperties = customProperties
            };
        }

        public IStreamEventRecordBuilder WithCustomProperties(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            foreach (var kv in keyValuePairs)
            {
                customProperties.Add(kv.Key, kv.Value);
            }
            return this;
        }

        public IStreamEventRecordBuilder WithRecord(IStreamEventRecord record)
        {
           return WithId(record.Id)
                 .Dated(record.Timestamp)
                 .WithData(record.Data)
                 .WithRevision(record.Revision)
                 .WithCustomProperties(record.CustomProperties);
        }
    }
}
