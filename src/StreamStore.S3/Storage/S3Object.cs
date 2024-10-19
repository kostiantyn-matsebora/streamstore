using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;


namespace StreamStore.S3.Storage
{

    abstract class S3Object : IS3Object
    {
        readonly S3ContainerPath path;
        readonly IS3ClientFactory clientFactory;

        public S3ObjectState State { get; private set; } = S3ObjectState.Unknown;

        protected S3Object(S3ContainerPath path, IS3ClientFactory clientFactory)
        {
            this.path = path;
            this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public virtual async Task DeleteAsync(CancellationToken token)
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
                    State = S3ObjectState.DoesNotExist;
                } else
                {
                    fileId = null;
                }
            } while (fileId != null);

            ResetState();
        }

        protected async Task<byte[]> LoadDataAsync(CancellationToken token)
        {
            await using var client = clientFactory.CreateClient();

            var eventResponse = await client!.FindObjectAsync(path, token);
            if (eventResponse != null)
            {
              
                State = S3ObjectState.Loaded;
                return eventResponse.Data!;
   
            } else
            {
                State = S3ObjectState.DoesNotExist;
                return null!;
            }
           
        }

        protected  async Task UploadDataAsync(byte[] data, CancellationToken token)
        {
            await using var client = clientFactory.CreateClient();

            var request = new UploadObjectRequest
            {
                Key = path,
                Data = data
            };
            await client!.UploadObjectAsync(request, token);
            State = S3ObjectState.Loaded;
        }

        public virtual void ResetState()
        {
            State = S3ObjectState.Unknown;
        }

        public abstract Task UploadAsync(CancellationToken token);

        public abstract Task LoadAsync(CancellationToken token);

    }
}
