using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;


namespace StreamStore.S3.Storage
{

    class S3Object : IS3Object
    {
        readonly S3ContainerPath path;
        readonly IS3ClientFactory clientFactory;

        public virtual byte[] Data { get; set; } = new byte[0];

        public Id Id => path.Name;

        public S3ObjectState State { get; private set; } = S3ObjectState.Unknown;

        public S3Object(S3ContainerPath path, IS3ClientFactory clientFactory)
        {
            this.path = path;
            this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public async Task DeleteAsync(CancellationToken token)
        {
            await using var client = clientFactory.CreateClient();

            string? fileId = null;
            do
            {
                var descriptor = await client.FindObjectDescriptorAsync(path, token);
                if (descriptor != null)
                {
                    fileId = descriptor!.FileId!;
                    await client.DeleteObjectByFileIdAsync(fileId!, descriptor.FileName!, token);
                    State = S3ObjectState.NotExists;
                } else
                {
                    fileId = null;
                }
            } while (fileId != null);
        }

        public async Task LoadAsync(CancellationToken token)
        {
            await using var client = clientFactory.CreateClient();

            var eventResponse = await client!.FindObjectAsync(path, token);
            if (eventResponse != null)
            {
                Data = eventResponse.Data!;
                State = S3ObjectState.Loaded;
            } else
            {
                State = S3ObjectState.NotExists;
            }
        }

        public async Task UploadAsync(CancellationToken token)
        {
            await using var client = clientFactory.CreateClient();

            var request = new UploadObjectRequest
            {
                Key = path,
                Data = Data
            };
            await client!.UploadObjectAsync(request, token);
            State = S3ObjectState.Loaded;
        }

        public void ResetState()
        {
            State = S3ObjectState.Unknown;
            Data = Enumerable.Empty<byte>().ToArray();
        }
    }
}
