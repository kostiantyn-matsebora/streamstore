using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using StreamStore.S3.Client;


namespace StreamStore.S3.Storage
{
    internal abstract class S3ObjectContainer<T>: IEnumerable<T>
    {
        readonly ConcurrentDictionary<string, T> objects = new ConcurrentDictionary<string, T>();

        protected S3ContainerPath path { get; }
        protected IS3ClientFactory clientFactory { get; }

        protected S3ObjectContainer(S3ContainerPath path, IS3ClientFactory clientFactory)
        {
            this.path = path;
            this.clientFactory = clientFactory ?? throw new System.ArgumentNullException(nameof(clientFactory));
        }

        public T GetChild(string name)
        {
            return objects.GetOrAdd(name, CreateChild);
        }

        public abstract T CreateChild(string name);


        public IEnumerator<T> GetEnumerator()
        {
           return objects.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return objects.Values.GetEnumerator();
        }

        public virtual void ResetState()
        {
            objects.Clear();
        }
    }

    internal class S3ObjectContainer : S3ObjectContainer<S3ObjectContainer>
    {
        protected S3ObjectContainer(S3ContainerPath path, IS3ClientFactory clientFactory) : base(path, clientFactory)
        {
        }

        public override S3ObjectContainer CreateChild(string name)
        {
            return new S3ObjectContainer(path.Combine(name), clientFactory);
        }
    }
}
