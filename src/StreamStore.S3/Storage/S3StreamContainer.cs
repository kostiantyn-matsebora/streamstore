﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StreamStore.S3.Client;
using StreamStore.Storage;



namespace StreamStore.S3.Storage
{
    internal class S3StreamContainer : S3ObjectStorage<S3MetadataObject, S3EventStorage>
    {
        public S3EventStorage Events { get; }

        public S3MetadataObject MetadataObject { get; }

        public StreamEventMetadataRecordCollection EventsMetadata => MetadataObject.Events!;

        public bool NotEmpty => Events.NotEmpty;
            
        public S3ObjectState State => MetadataObject.State;

        public S3StreamContainer(S3ContainerPath path, IS3ClientFactory clientFactory) : base(path, clientFactory)
        {

            Events = GetContainer("events");
            MetadataObject = GetItem("__metadata");
        }

        public async Task LoadAsync(Revision startFrom, int count, CancellationToken token = default)
        {
            await MetadataObject.LoadAsync(token);

            if (MetadataObject.State == S3ObjectState.DoesNotExist) return;

            if (startFrom > MetadataObject.Events.MaxRevision)
            {
                return;
            }

            var ids = MetadataObject.Events
                .Where(e => e.Revision >= startFrom)
                .Take(count)
                .Select(e => e.Id)
                .ToList();

            var tasks = ids.Select(async id =>
            {
                await Events.LoadEventAsync(id, token);
            });

            await Task.WhenAll(tasks);
        }

        public async Task AppendEventAsync(IStreamEventRecord record, CancellationToken token)
        {
            await Events.AppendAsync(record, token);
            await MetadataObject.AppendEventAsync(record, token).UploadAsync(token);
        }

        public async override Task DeleteAsync(CancellationToken token)
        {
            // Delete metadata
            await MetadataObject.DeleteAsync(token);

            // Delete all events
            await Events.DeleteAsync(token);

            ResetState();
        }

        public async Task CopyFrom(S3StreamContainer source, CancellationToken token)
        {

            // Copying events from source
            var tasks = source.Events.Select(async @event =>
            {
                var item = Events.GetItem(@event.Event!.Id);
                await item.ReplaceByAsync(@event, token);
            });

            Task.WaitAll(tasks.ToArray());

            // Replacing metadata
            await MetadataObject.ReplaceByAsync(source.MetadataObject, token);
        }

        protected override S3MetadataObject CreateItem(string name)
        {
           return new S3MetadataObject(path.Combine(name), clientFactory);
        }

        protected override S3EventStorage CreateContainer(string name)
        {
            return new S3EventStorage(path.Combine(name), clientFactory);
        }
    }
}
