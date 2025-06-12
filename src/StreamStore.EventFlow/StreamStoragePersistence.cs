using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Core;
using EventFlow.EventStores;
using StreamStore.Extensions;
using EventFlow.Exceptions;
namespace StreamStore.EventFlow
{
    internal class StreamStoragePersistence : IEventPersistence
    {
        readonly IStreamStorage storage;
        readonly IJsonSerializer serializer;

        public StreamStoragePersistence(IStreamStorage storage, IJsonSerializer serializer)
        {
            this.storage = storage.ThrowIfNull(nameof(storage));
            this.serializer = serializer.ThrowIfNull(nameof(serializer));
        }

        public async Task<IReadOnlyCollection<ICommittedDomainEvent>> CommitEventsAsync(IIdentity id, IReadOnlyCollection<SerializedEvent> serializedEvents, CancellationToken cancellationToken)
        {
            if (serializedEvents == null || serializedEvents.Count == 0)
            {
                throw new ArgumentException("Serialized events cannot be null or empty", nameof(serializedEvents));
            }
            try
            {
                await storage.WriteAsync(id.Value, ToEventRecords(serializedEvents), cancellationToken);
            } catch (Exceptions.Appending.OptimisticConcurrencyException ex)
            {
                throw new OptimisticConcurrencyException($"Failed to commit events for stream {id.Value}. Revision already exists.", ex);
            }

            return new List<ICommittedDomainEvent>(ToCommittedDomainEvents(serializedEvents));
        }

        public async Task DeleteEventsAsync(IIdentity id, CancellationToken cancellationToken)
        {
            await storage.DeleteAsync(id.Value, cancellationToken);
        }

        public Task<AllCommittedEventsPage> LoadAllCommittedEvents(GlobalPosition globalPosition, int pageSize, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("Loading all commited events is not supported by StreamStore");
        }

        public async Task<IReadOnlyCollection<ICommittedDomainEvent>> LoadCommittedEventsAsync(IIdentity id, int fromEventSequenceNumber, CancellationToken cancellationToken)
        {
            var metadata = await storage.GetMetadataAsync(id.Value, cancellationToken);

            if (metadata == null)
                return Array.Empty<ICommittedDomainEvent>();

            return await LoadCommittedEventsAsync(id, fromEventSequenceNumber, metadata.Revision.Value , cancellationToken);
        }

        public async Task<IReadOnlyCollection<ICommittedDomainEvent>> LoadCommittedEventsAsync(IIdentity id, int fromEventSequenceNumber, int toEventSequenceNumber, CancellationToken cancellationToken)
        {

            var events = await storage.ReadAsync(id.Value, fromEventSequenceNumber, toEventSequenceNumber - fromEventSequenceNumber + 1, cancellationToken);
            var result =  ToCommittedDomainEvents(id, events.Where(e => e.Revision <= toEventSequenceNumber)).ToList();
            return result;
        }

        IEnumerable<IStreamEventRecord> ToEventRecords(IReadOnlyCollection<SerializedEvent> serializedEvents)
        {
            foreach (var serializedEvent in serializedEvents)
            {
                yield return
                    new StreamEventRecord
                    {
                        Data = Encoding.UTF8.GetBytes(serializedEvent.SerializedData),
                        CustomProperties = serializedEvent.Metadata,
                        Id = serializedEvent.Metadata!.EventId.Value,
                        Timestamp = serializedEvent.Metadata.Timestamp.UtcDateTime,
                        Revision = serializedEvent.AggregateSequenceNumber
                    };
            }
        }

        IEnumerable<ICommittedDomainEvent> ToCommittedDomainEvents(IReadOnlyCollection<SerializedEvent> serializedEvents)
        {
            foreach (var serializedEvent in serializedEvents)
            {
                yield return
                    new CommittedDomainEvent
                    {
                        AggregateId = serializedEvent.Metadata!.AggregateId,
                        Data = serializedEvent.SerializedData,
                        Metadata = serializedEvent.SerializedMetadata,
                        AggregateSequenceNumber = serializedEvent.AggregateSequenceNumber
                    };
            }
        }

        IEnumerable<ICommittedDomainEvent> ToCommittedDomainEvents(IIdentity streamId, IEnumerable<IStreamEventRecord> events)
        {
            foreach (var @event in events)
            {
                yield return
                    new CommittedDomainEvent
                    {
                        AggregateId = streamId.Value,
                        Data = Encoding.UTF8.GetString(@event.Data),
                        Metadata = serializer.Serialize(@event.CustomProperties?? new Dictionary<string,string>()),
                        AggregateSequenceNumber = @event.Revision
                    };
            }
        }
    }
}
