using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


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
                
                if (blobMetadata.Revision!= expectedRevision)
                    new OptimisticConcurrencyException(expectedRevision, revision, id);
            }

            using var lockObject = factory.CreateLock(id);
            var acquired = await lockObject.AcquireAsync(cancellationToken);
            
            if (!acquired)
                throw new StreamAlreadyLockedException(id);

            try
            {
                // Get the current revision
                blobMetadata = await client.FindBlobMetadataAsync(id, cancellationToken);
                if (blobMetadata != null)
                {
                    if (blobMetadata.Revision != expectedRevision)
                        new OptimisticConcurrencyException(expectedRevision, revision, id);
                }

                // Upload events
                var data =
                    System.Text.Encoding.UTF8.GetBytes(
                        Newtonsoft.Json.JsonConvert.SerializeObject(
                            new StreamRecord(id, records)));

                await client.UploadBlobAsync(id, data, new StreamMetadata(id, revision), cancellationToken);

            }
            finally
            {
                await lockObject.ReleaseAsync(cancellationToken);
            }
        }
    }
}
