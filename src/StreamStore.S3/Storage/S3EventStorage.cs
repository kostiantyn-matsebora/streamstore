﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;


namespace StreamStore.S3.Storage
{
    internal class S3EventStorage : S3ObjectStorage<S3EventObject, S3ObjectContainer>
    {

        public bool HasChanges => objects.Any();

        public S3EventStorage(S3ContainerPath path, IS3ClientFactory clientFactory) : base(path, clientFactory)
        {
        }

        public async Task AppendAsync(EventRecord record, CancellationToken token)
        {
            var @event = GetItem(record.Id);
            await @event.ReplaceBy(record).UploadAsync(token);

        }

        public async Task<S3EventObject> LoadEventAsync(Id eventId, CancellationToken token)
        {
            var @event = GetItem(eventId);
            await @event.LoadAsync(token);
            return @event;
        }

        protected override S3ObjectContainer CreateContainer(string name)
        {
            return base.GetChildContainer(name);
        }

        protected override S3EventObject CreateItem(string name)
        {
            return new S3EventObject(path.Combine(name), clientFactory);
        }
    }
}
