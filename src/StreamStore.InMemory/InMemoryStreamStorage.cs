using System.Threading.Tasks;

using System.Threading;
using System.Collections.Concurrent;
using StreamStore.Exceptions;
using System;
using System.Linq;
using StreamStore.Storage;
using System.Collections.Generic;
using StreamStore.Validation;


namespace StreamStore.InMemory
{

    public sealed class InMemoryStreamStorage : IStreamStorage
    {
        readonly IDuplicateEventValidator eventValidator;
        private readonly IDuplicateRevisionValidator revisionValidator;
        internal ConcurrentDictionary<string, List<IStreamEventRecord>> storage;
        
        public InMemoryStreamStorage(IDuplicateEventValidator eventValidator, IDuplicateRevisionValidator revisionValidator)
        {
            this.eventValidator = eventValidator.ThrowIfNull(nameof(eventValidator));
            this.revisionValidator = revisionValidator.ThrowIfNull(nameof(revisionValidator));
            storage = new ConcurrentDictionary<string, List<IStreamEventRecord>>();
        }

       

        public Task DeleteAsync(Id streamId, CancellationToken token = default)
        {
            if (storage.ContainsKey(streamId))
                storage.TryRemove(streamId, out var _);

            return Task.CompletedTask;
        }

        public Task<IStreamMetadata?> GetMetadataAsync(Id streamId, CancellationToken token = default)
        {
            if (!storage.TryGetValue(streamId, out var record))
                return Task.FromResult<IStreamMetadata?>(null!);

            var events = new StreamEventMetadataRecordCollection(record);

            return  
                Task.FromResult<IStreamMetadata?>(new StreamMetadata(streamId, events.MaxRevision, events.LastModified.GetValueOrDefault()));
        }

        public async Task<IStreamEventRecord[]> ReadAsync(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            if (startFrom < Revision.One)
                throw new ArgumentOutOfRangeException(nameof(startFrom), "Start from should be greater than zero.");

            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count should be greater than zero.");

            var stream = await FindAsync(streamId);

            if (stream == null)
                throw new StreamNotFoundException(streamId);

            if (startFrom > stream.MaxRevision())
                return Array.Empty<IStreamEventRecord>();

            return
                    stream
                        .Where(e => e.Revision >= startFrom)
                        .Take(count).ToArray();
        }

        public Task WriteAsync(Id streamId, IEnumerable<IStreamEventRecord> batch, CancellationToken token = default)
        {
            var record = new List<IStreamEventRecord>(batch);

            storage.AddOrUpdate(streamId, record, (key, oldValue) =>
            {
                var request = new StreamMutationRequest(key, record.ToArray(), oldValue);
                var result = eventValidator.Validate(request.Events);
                if (!result.IsValid)
                    throw new EventAlreadyExistsException(key);

                result = revisionValidator.Validate(request.Events);
                if (!result.IsValid)
                    throw new RevisionAlreadyExistsException(key);

                oldValue.AddRange(record);
                return oldValue;
            });

            return Task.CompletedTask;
        }


        Task<List<IStreamEventRecord>?> FindAsync(Id streamId)
        {
            if (!storage.TryGetValue(streamId, out var record))
                return Task.FromResult<List<IStreamEventRecord>?>(null);

            return Task.FromResult<List<IStreamEventRecord>?>(record);
        }
    }
}
