using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;


namespace StreamStore.S3.Operations
{
    internal sealed class S3StreamDeleter
    {
        readonly IS3Client? client;
        readonly Id streamId;

        public S3StreamDeleter(Id streamId, IS3Client client)
        {
            if (streamId == Id.None)
                throw new ArgumentException("StreamId cannot be empty", nameof(streamId));
            this.streamId = streamId;
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task DeleteAsync(CancellationToken token)
        {
            //Delete all objects from container
            await client!.DeleteObjectAsync(S3Naming.StreamPrefix(streamId), null, token);
        }

        public static S3StreamDeleter New(Id streamId, IS3Client client)
        {
            return new S3StreamDeleter(streamId, client);
        }
    }
}
