using System;


namespace StreamStore.Storage
{
    class StreamEventRecordBuilder : IStreamEventRecordBuilder
    {
        Id id;
        DateTime timestamp = DateTime.Now;
        byte[] data = null!;
        Revision revision;
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
            if (revision == Revision.Zero)
                throw new ArgumentOutOfRangeException(nameof(revision));

            return new StreamEventRecord
            {
                Id = id,
                Timestamp = timestamp,
                Data = data,
                Revision = revision
            };
        }
    }
}
