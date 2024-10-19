using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;


namespace StreamStore.S3.Storage
{
    internal class S3ObjectContainer
    {
        protected readonly ConcurrentDictionary<string, S3Object> objects = new ConcurrentDictionary<string, S3Object>();
        protected readonly ConcurrentDictionary<string, S3ObjectContainer> containers = new ConcurrentDictionary<string, S3ObjectContainer>();

        protected S3ContainerPath path { get; }
        protected IS3ClientFactory clientFactory { get; }

        public S3ObjectContainer(S3ContainerPath path, IS3ClientFactory clientFactory)
        {
            this.path = path;
            this.clientFactory = clientFactory ?? throw new System.ArgumentNullException(nameof(clientFactory));
        }

        public virtual async Task DeleteAsync(CancellationToken token)
        {
            await using (var client = clientFactory.CreateClient())
            {

                string? startObjectName = null;
                do
                {
                    if (token.IsCancellationRequested) return;

                    var childObjects = await GetChildObjects(client, startObjectName);

                    if (childObjects!.Length == 0) return;

                    startObjectName = DeleteChildObjects(client, startObjectName, childObjects);

                } while (startObjectName != null);
            }

            ResetState();
        }

        public virtual void ResetState()
        {
            foreach (var obj in objects.Values)
            {
                obj.ResetState();
            }
            foreach (var container in containers.Values)
            {
                container.ResetState();
            }

            containers.Clear();
            objects.Clear();
        }

        static string? DeleteChildObjects(IS3Client client, string? startObjectName, ObjectDescriptor[] childObjects)
        {
            var tasks = childObjects.Select(async e =>
            {
                await client.DeleteObjectByFileIdAsync(e.FileId!, e.FileName!, CancellationToken.None);
                startObjectName = e.FileName;
            });
            Task.WaitAll(tasks.ToArray());
            return startObjectName;
        }

        async Task<ObjectDescriptor[]> GetChildObjects(IS3Client client, string? startObjectName)
        {
            var response = await client.ListObjectsAsync(path.Normalize(), startObjectName, CancellationToken.None);
            if (response == null) return Enumerable.Empty<ObjectDescriptor>().ToArray();
            return response.Objects!;
        }
    }
}

