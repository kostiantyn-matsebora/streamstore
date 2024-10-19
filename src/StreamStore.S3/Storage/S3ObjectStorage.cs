using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using StreamStore.S3.Client;


namespace StreamStore.S3.Storage
{
    internal abstract class S3ObjectStorage<T, TContainer> : S3ObjectContainer, IEnumerable<T>
        where T : S3Object
        where TContainer : S3ObjectContainer
    {
        protected S3ObjectStorage(S3ContainerPath parent, IS3ClientFactory clientFactory) : base(parent, clientFactory)
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
           return objects.Where(x => x.Value is T).Select(x => (T)x.Value).GetEnumerator();
        }

        public T GetItem(string name)
        {
            return (T)objects.GetOrAdd(name, _ => CreateItem(_));
        }
        public TContainer GetContainer(string name)
        {
            return (TContainer)containers.GetOrAdd(name, _ => CreateContainer(_));
        }

        protected abstract T CreateItem(string name);

        protected abstract TContainer CreateContainer(string name);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        public async Task DeleteAsync(Id streamId, CancellationToken token = default)
        {
            var item = GetItem(streamId);
            await item.DeleteAsync(token);
        }

        public async Task DeleteContainerAsync(Id streamId, CancellationToken token = default)
        {
            var item = GetContainer(streamId);
            await item.DeleteAsync(token);
        }
    }

    internal abstract class S3ObjectStorage<TContainer> : S3ObjectStorage<S3BinaryObject, TContainer> where TContainer : S3ObjectContainer
    {
        protected S3ObjectStorage(S3ContainerPath parent, IS3ClientFactory clientFactory) : base(parent, clientFactory)
        {
        }

        protected override S3BinaryObject CreateItem(string name)
        {
            return new S3BinaryObject(path.Combine(name), clientFactory);
        }
    }
}
