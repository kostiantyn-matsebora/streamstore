using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;
using StreamStore.Serialization;


namespace StreamStore.S3.Storage
{
    class S3LockObject : S3Object
    {
        LockId? lockId;
        public LockId? LockId => lockId;

        public S3LockObject(S3ContainerPath path, IS3ClientFactory clientFactory) : base(path, clientFactory)
        {
        }

        public override async Task LoadAsync(CancellationToken token)
        {
            await base.LoadAsync(token);
            if (State == S3ObjectState.Loaded) lockId = Converter.FromByteArray<LockId>(Data)!;
        }

        public override async Task DeleteAsync(CancellationToken token)
        {
            await base.DeleteAsync(token);
            lockId = null;
        }

        public void ReplaceBy(LockId lockId)
        {
            lockId = lockId;
            Data = Converter.ToByteArray(lockId!);
        }

        public override void ResetState()
        {
            base.ResetState();
            lockId = null;
        }
    }
}
