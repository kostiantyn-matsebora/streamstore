using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EventFlow.Core;
using EventFlow.EventStores;
using StreamStore.Storage;


namespace StreamStore.EventFlow
{
    internal class StreamStorageEventPersistence : IEventPersistence
    {
        readonly IStreamStorage storage;
        private readonly IMapper mapper;

        public StreamStorageEventPersistence(IStreamStorage storage, IMapper mapper)
        {
          this.storage = storage.ThrowIfNull(nameof(storage));
          this.mapper = mapper.ThrowIfNull(nameof(mapper));
        }

        public async Task<IReadOnlyCollection<ICommittedDomainEvent>> CommitEventsAsync(IIdentity id, IReadOnlyCollection<SerializedEvent> serializedEvents, CancellationToken cancellationToken)
        {
            var records = serializedEvents.Select(e => mapper.Map<SerializedEvent, IStreamEventRecord>(e)).ToArray();
            var metadata  = new StreamEventMetadataRecordCollection(records);
            //foreach (var serializedEvent in serializedEvents)
            //{ 
            //    mapper.Map<>
            //    IMetadata metadata = serializedEvent.Metadata;
              
            //    var builder = new StreamEventRecordBuilder();
            //    builder.WithId(metadata.EventId.Value)
            //            .Dated(metadata.Timestamp.DateTime)
            //            .WithRevision(serializedEvent.AggregateSequenceNumber)
            //            .WithData(Encoding.UTF8.GetBytes(serializedEvent.SerializedData))
            //            .WithCustomProperties(metadata);

            //    records.Add(builder.Build());
            //}

            await storage.WriteAsync(id.Value, records, cancellationToken);
            return await LoadCommittedEventsAsync(id, metadata.MinRevision, metadata.MaxRevision, cancellationToken);
        }

        public async Task DeleteEventsAsync(IIdentity id, CancellationToken cancellationToken)
        {
            await storage.DeleteAsync(id.Value, cancellationToken);
        }

        public Task<AllCommittedEventsPage> LoadAllCommittedEvents(GlobalPosition globalPosition, int pageSize, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<ICommittedDomainEvent>> LoadCommittedEventsAsync(IIdentity id, int fromEventSequenceNumber, CancellationToken cancellationToken)
        {
            var metadata = await storage.GetMetadataAsync(id.Value, cancellationToken);
            if (metadata == null || fromEventSequenceNumber > metadata.Revision)
                return Array.Empty<ICommittedDomainEvent>();

            return await LoadCommittedEventsAsync(id, fromEventSequenceNumber, metadata.Revision, cancellationToken);
        }

        public async Task<IReadOnlyCollection<ICommittedDomainEvent>> LoadCommittedEventsAsync(IIdentity id, int fromEventSequenceNumber, int toEventSequenceNumber, CancellationToken cancellationToken)
        {
         return (await storage.ReadAsync(id.Value, fromEventSequenceNumber, toEventSequenceNumber, cancellationToken))
                .Select(e => mapper.Map<IStreamEventRecord, ICommittedDomainEvent>(e))
                .ToArray();
        }
    }
}
