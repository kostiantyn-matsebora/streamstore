using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StreamStore.S3
{
    public class S3StreamDatabase : IStreamDatabase
    {
        private IS3ClientFactory clientFactory;
        private S3StreamDatabaseSettings settings;

        public S3StreamDatabase(IS3ClientFactory clientFactory, S3StreamDatabaseSettings settings)
        {
            if (clientFactory == null)
                throw new ArgumentNullException(nameof(clientFactory));
            this.clientFactory = clientFactory;
            this.settings = settings;
        }

        public IStreamUnitOfWork BeginAppend(string streamId, int expectedStreamVersion = 0)
        {
            return new S3StreamUnitOfWork(streamId, expectedStreamVersion, clientFactory.CreateClient(), settings);
        }

        public async Task DeleteAsync(string streamId, CancellationToken cancellationToken)
        {
            using var client = clientFactory.CreateClient();
            await client.DeleteBlobAsync(settings.BucketName!, streamId, cancellationToken);
        }

        public async Task<StreamRecord?> FindAsync(string streamId, CancellationToken cancellationToken)
        {
            using  var client = clientFactory.CreateClient();
            using var stream = await client.FindBlobAsync(settings.BucketName!, streamId, cancellationToken);
            if (stream ==null)
                return null;

              
            var data = System.Text.Encoding.UTF8.GetString(stream.ToArray());
            StreamRecord? record = JsonConvert.DeserializeObject<StreamRecord>(data);
            return record;
        }

        public async Task<StreamMetadataRecord?> FindMetadataAsync(string streamId, CancellationToken cancellationToken)
        {
            var record = await FindAsync(streamId, cancellationToken);
            if (record == null)
                return null;

            return new StreamMetadataRecord(record.Id, record.Events);
        }
    }
}