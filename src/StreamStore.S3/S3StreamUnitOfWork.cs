using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;


namespace StreamStore.S3
{
    public class S3StreamUnitOfWork : IStreamUnitOfWork
    {        
        readonly List<EventRecord> records = new List<EventRecord>();
        readonly Id id;
        readonly int expectedRevision;
        readonly S3AbstractFactory factory;

        public S3StreamUnitOfWork(Id streamId, int expectedRevision,  S3AbstractFactory factory)
        {

            if (streamId == Id.None)
                throw new ArgumentNullException(nameof(streamId));
            id = streamId;
            if (expectedRevision < 0)
                throw new ArgumentOutOfRangeException(nameof(expectedRevision));
            this.expectedRevision = expectedRevision;
            
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public IStreamUnitOfWork Add(Id eventId, int revision, DateTime timestamp, string data)
        {
            records.Add(
                new EventRecord {
                    Id = eventId,
                    Revision = revision,
                    Timestamp = timestamp,
                    Data = data
            });
            return this;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            using var client = factory.CreateClient();

            if (records.Count == 0)
                throw new InvalidOperationException("No events to save.");
            int revision = expectedRevision;

            var blobMetadata = await client.FindBlobMetadataAsync(id, cancellationToken);
            if (blobMetadata != null)
            {
                revision = Convert.ToInt32(blobMetadata["stream-revision"]);
                if (revision != expectedRevision)
                    new OptimisticConcurrencyException(expectedRevision, revision, id);
            }

            using var lockObject = factory.CreateLock(id);
            var acquired = await lockObject.AcquireAsync(cancellationToken);
            
            if (!acquired)
                throw new StreamLockedException(id);

            try
            {
                // Get the current revision
                blobMetadata = await client.FindBlobMetadataAsync(id, cancellationToken);
                if (blobMetadata != null)
                {
                    revision = Convert.ToInt32(blobMetadata["stream-revision"]);
                    if (revision != expectedRevision)
                        new OptimisticConcurrencyException(expectedRevision, revision, id);
                }

                // Upload events
                var data =
                    System.Text.Encoding.UTF8.GetBytes(
                        Newtonsoft.Json.JsonConvert.SerializeObject(
                            new StreamRecord(id, records)));

                var metadata = 
                    new S3MetadataCollection()
                    .Add("stream-revision", revision.ToString());

                await client.UploadBlobAsync(id, data, metadata, cancellationToken);

            }
            finally
            {
                await lockObject.ReleaseAsync(cancellationToken);
            }
        }
    }
}
