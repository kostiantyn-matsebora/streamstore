using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StreamStore.S3
{
    public sealed class S3StreamDatabase : IStreamDatabase
    {

        readonly S3AbstractFactory factory;
        public S3StreamDatabase(S3AbstractFactory factory)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public IStreamUnitOfWork BeginAppend(string streamId, int expectedStreamVersion = 0)
        {
            return new S3StreamUnitOfWork(streamId, expectedStreamVersion, factory);
        }

        public async Task DeleteAsync(string streamId, CancellationToken cancellationToken)
        {
            using var client = factory.CreateClient();
            await client.DeleteBlobAsync(streamId, cancellationToken);
        }

        public async Task<StreamRecord?> FindAsync(string streamId, CancellationToken cancellationToken)
        {
            using  var client = factory.CreateClient();
            var data = await client.FindBlobAsync(streamId, cancellationToken);
            if (data ==null)
                return null;

              
            var stream = System.Text.Encoding.UTF8.GetString(data);
            StreamRecord? record = JsonConvert.DeserializeObject<StreamRecord>(stream);
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