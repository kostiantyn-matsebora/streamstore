using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;


namespace StreamStore.S3.Operations
{
    internal class S3StreamCopier : OperationBase
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
            string? startObjectName = null;
            List<ObjectDescriptor> files;

            // Copy events
            do
            {
                var response = await client!.ListObjectsAsync(source.EventsKey, startObjectName, token);

                if (response == null) break;

                files = response.Objects!.ToList();
                files = files.Where(e => e.FileName != startObjectName).ToList();
                if (!files.Any()) break;

                startObjectName = files.Last().FileName;

                var tasks = files.Select(
                    async e =>
                    {
                        var destinationKey = CalculateDestinationKey(e.FileName!, source.EventsKey, destination.EventsKey);
                        await client.CopyByFileIdAsync(e.FileId!, destinationKey, token);
                     
                    });

                await Task.WhenAll(tasks);
            } while (files.Any());


            // Copy metadata
            var metadata = await client!.FindObjectDescriptorAsync(source.MetadataKey, token);
            if (metadata == null) return;

            await client!.CopyByFileIdAsync(metadata.FileId!, destination.MetadataKey, token);
        }

        public static S3StreamCopier New(S3StreamContext source, S3StreamContext destination, IS3Client client)
        {
            return new S3StreamCopier(source, destination, client);
        }
    }
}
