using System.Collections.Generic;
using System.Linq;


namespace StreamDB.Operations
{
    internal static class AppentToStreamInvariants
    {
        public static void ApplyAll(string streamId, IEnumerable<IEventMetadata> transient, IEnumerable<IEventMetadata>? persistent = null)
        {
            var transientBatch = new EventMetadataBatch(transient);

            var persistentBatch = new EventMetadataBatch(persistent);

            ThrowIfThereIsDuplicates(streamId, transientBatch, persistentBatch);
            ThrowIfConccurrencyConflict(streamId, transientBatch, persistentBatch);
        }

        static void ThrowIfThereIsDuplicates(string streamId, EventMetadataBatch transient, EventMetadataBatch persistent)
        {
            var ids = transient
                        .Select(e => e.Id)
                        .Concat(persistent.Select(e => e.Id))
                        .ToArray();

            var duplicates =
             ids.GroupBy(id => id)
                 .Where(g => g.Count() > 1)
                 .Select(g => g.Key)
                 .ToArray();

            if (duplicates.Any())
                throw new DuplicateEventException(duplicates, streamId);
        }

        static void ThrowIfConccurrencyConflict(string streamId, EventMetadataBatch transient, EventMetadataBatch persistent)
        {
            var expectedRevision = transient.MinRevision - 1;
            var actualRevision = persistent.MaxRevision;

            if (expectedRevision != actualRevision)
                throw new OptimisticConcurrencyException(
                    expectedRevision,
                    actualRevision,
                    streamId);
        }
    }
}
