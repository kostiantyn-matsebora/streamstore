using System.Collections;
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

        protected S3Object GetChildObject(string name)
        {
            return objects.GetOrAdd(name, _ => new S3Object(path.Combine(_), clientFactory));
        }

        protected S3ObjectContainer GetChildContainer(string name)
        {
            return containers.GetOrAdd(name, _ => new S3ObjectContainer(path.Combine(_), clientFactory));
        }

        public virtual async Task DeleteAsync(CancellationToken token)
        {
            await using (var client = clientFactory.CreateClient())
            {

                string? startObjectName = null;
                do
                {
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
    }
}

