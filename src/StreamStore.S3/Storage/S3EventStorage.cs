using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;


namespace StreamStore.S3.Storage
{
    internal class S3EventStorage : S3ObjectStorage<S3EventObject>
    {
        public S3EventStorage(S3ContainerPath path, IS3ClientFactory clientFactory) : base(path, clientFactory)
        {
        }

        public override S3EventObject CreateChild(string name)
        {
            return new S3EventObject(path.Combine(name), clientFactory);
        }

        public async Task AppendAsync(EventRecord record, CancellationToken token)
        {
            var @event = GetChild(record.Id);
            @event.Event = record;
            await @event.UploadAsync(token);
        }

        public async Task DeleteAsync(CancellationToken token)
        {
            await using (var client = clientFactory.CreateClient())
            {

                string? startObjectName = null;
                do
                {
                    var directoryPath = path.Normalize();
                    var response = await client.ListObjectsAsync(path.Normalize(), startObjectName, token);
                    if (response == null) return;

                    if (response!.Objects!.Length == 0) return;

                    var tasks = response.Objects.Select(async e =>
                    {
                        await client.DeleteObjectByFileIdAsync(e.FileId!, e.FileName!, token);
                        startObjectName = e.FileName;
                    });
                    Task.WaitAll(tasks.ToArray());
                } while (startObjectName != null);

            }
        }

        public async Task<S3EventObject> LoadAsync(Id eventId, CancellationToken token)
        {
            var @event = GetChild(eventId);
            await @event.LoadAsync(token);
            return @event;
        }
    }
}
