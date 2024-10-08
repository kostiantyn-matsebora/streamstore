using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;


namespace StreamStore.S3.Operations
{
    internal class S3StreamCopier
    {
        readonly S3StreamContext source;
        readonly S3StreamContext destination;
        readonly IS3Client client;

        S3StreamCopier(S3StreamContext source, S3StreamContext destination, IS3Client client)
        {
            this.source = source ?? throw new System.ArgumentNullException(nameof(source));
            this.destination = destination ?? throw new System.ArgumentNullException(nameof(destination));
            this.client = client ?? throw new System.ArgumentNullException(nameof(client));
        }

        public async Task CopyAsync(CancellationToken token)
        {
            // First copy events
            await client.CopyAsync(source.EventsKey, destination.EventsKey, token);

            // Then copy metadata
            await client.CopyAsync(source.MetadataKey, destination.MetadataKey, token);
        }

        public static S3StreamCopier New(S3StreamContext source, S3StreamContext destination, IS3Client client)
        {
            return new S3StreamCopier(source, destination, client);
        }
    }
}
