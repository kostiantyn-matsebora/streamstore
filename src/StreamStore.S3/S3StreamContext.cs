
using System;

namespace StreamStore.S3
{

    internal abstract class S3StreamContext
    {
        protected readonly string streamContainer;
        protected readonly Id streamId;

        internal const string Delimiter = "/";


        public string StreamKey => $"{streamContainer}{Delimiter}{StreamId}{Delimiter}";


        public string EventKey(Id eventId) => $"{EventsKey}{eventId}";

        public string MetadataKey => $"{StreamKey}__metadata";

        public string EventsKey => $"{StreamKey}events{Delimiter}";

        protected S3StreamContext(Id streamId, string streamContainer)
        {
            this.streamContainer = streamContainer;
            this.streamId = streamId;
        }

        public abstract string StreamId { get; }

        public static S3StreamContext Persistent(Id streamId) => new S3PersistentContext(streamId);
        public static S3StreamContext Transient(Id streamId, Id transactionId) => new S3TransientContext(streamId, transactionId);

        class S3PersistentContext : S3StreamContext
        {
            public S3PersistentContext(Id streamId) : base(streamId, "persistent-streams")
            {
            }

            public override string StreamId => this.streamId;
        }

        class S3TransientContext : S3StreamContext
        {
            readonly Id transactionId;
            public S3TransientContext(Id streamId, Id transactionId) : base(streamId, "transient-streams")
            {
                this.transactionId = transactionId;
            }

            public override string StreamId => $"{this.streamId}{Delimiter}{transactionId}";
        }
    }
}
