using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace StreamStore.S3
{
    public class S3StreamUnitOfWork : IStreamUnitOfWork
    {
        readonly IS3Client client;
        readonly S3StreamDatabaseSettings settings;
        readonly List<EventRecord> records = new List<EventRecord>();
        readonly Id id;
        readonly int expectedRevision;

        public S3StreamUnitOfWork(Id streamId, int expectedRevision, IS3Client client, S3StreamDatabaseSettings settings)
        {
            if (streamId == Id.None)
                throw new ArgumentException("StreamId must be specified.", nameof(streamId));
            id = streamId;
            this.expectedRevision = expectedRevision;
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            this.client = client;
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            this.settings = settings;
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
            if (records.Count == 0)
                throw new InvalidOperationException("No events to save.");
            int revision = expectedRevision;

            var metadata = await client.FindBlobMetadataAsync(settings.BucketName!, id, cancellationToken);
            if (metadata != null)
            {
                revision = Convert.ToInt32(metadata["stream-revision"]);
                if (revision != expectedRevision)
                    new OptimisticConcurrencyException(expectedRevision, revision, id);
            }

            // TODO: Add lock mechanism here

            // Upload events
            using var memoryStream = 
                new MemoryStream(
                System.Text.Encoding.UTF8.GetBytes(
                    Newtonsoft.Json.JsonConvert.SerializeObject(
                        new StreamRecord(id, records))));

            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "stream-revision", revision.ToString() }
            };

            await client.UploadBlobAsync(settings.BucketName!, id, memoryStream, data, cancellationToken);
        }
    }
}
