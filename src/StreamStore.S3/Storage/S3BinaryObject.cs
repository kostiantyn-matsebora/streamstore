using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;


namespace StreamStore.S3.Storage
{
    class S3BinaryObject : S3Object
    {
        public S3BinaryObject(S3ContainerPath path, IS3ClientFactory clientFactory) : base(path, clientFactory)
        {
        }

        public byte[]? Data { get; private set; }

        public override async Task LoadAsync(CancellationToken token)
        {
            Data = await LoadDataAsync(token);
        }

        public override async Task UploadAsync(CancellationToken token)
        {
           await UploadDataAsync(Data!, token);
        }
        public override void ResetState()
        {
            base.ResetState();
            Data = null;
        }
    }
}
