using System;
using System.Collections.Generic;
using System.Linq;
using StreamStore.S3.Models;


namespace StreamStore.S3.Concurrency
{
    class S3TransactionContext : IS3TransactionContext
    {
        readonly EventRecordCollection uncommited = new EventRecordCollection();
        readonly S3EventMetadataCollection uncommitedMetadata = new S3EventMetadataCollection();
        readonly Id transactionId;

        S3TransactionContext(Id streamId, Id transactionId)
        {
            Transient = S3StreamContext.Transient(streamId, transactionId);
            Persistent = S3StreamContext.Persistent(streamId);
            StreamId = streamId;
            this.transactionId = transactionId;
        }

        public S3StreamContext Transient { get; }
        public S3StreamContext Persistent { get; }
        public Id TransactionId => transactionId;
        public Id StreamId { get; }

        public bool HasChanges => Uncommited.Any();

        public IEnumerable<EventRecord> Uncommited => uncommited;

        public IEnumerable<S3EventMetadata> UncommitedMetadata => uncommitedMetadata;

        public string LockKey => $"locks{S3StreamContext.Delimiter}{StreamId}";

        public S3TransactionContext Add(EventRecord eventRecord)
        {
            uncommited.Add(eventRecord);
            uncommitedMetadata.Add(eventRecord.ToMetadata());
            return this;
        }
        public static S3TransactionContext New(Id streamId)
        {
            return new S3TransactionContext(streamId, Guid.NewGuid().ToString());
        }
    }
}
