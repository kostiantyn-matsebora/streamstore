using System;
using System.Collections.Generic;
using System.Linq;


namespace StreamStore
{
    internal class Validator
    {
        EventMetadataBatch? transient;
        EventMetadataBatch? persistent;
        string? streamId;

        public Validator Uncommited(IEnumerable<IEventMetadata> uncommited)
        {
            if (uncommited == null)
                throw new ArgumentNullException(nameof(uncommited));

            transient = new EventMetadataBatch(uncommited);

            return this;
        }

        public Validator Persistent(IEnumerable<IEventMetadata>? persistent)
        {
            this.persistent = new EventMetadataBatch(persistent);

            return this;
        }

        public Validator StreamId(string streamId)
        {
            this.streamId = streamId;

            return this;
        }

        public void Validate()
        {
            ThrowIfThereIsDuplicates();
            ThrowIfConccurrencyConflict();
        }

        public void ThrowIfThereIsDuplicates()
        {
            ValidateInput();

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
                throw new DuplicateEventException(duplicates, streamId!);
        }

        public void ThrowIfConccurrencyConflict()
        {
            ValidateInput();

            var expectedRevision = transient!.MinRevision - 1;
            var actualRevision = persistent!.MaxRevision;

            if (expectedRevision != actualRevision)
                throw new OptimisticConcurrencyException(
                    expectedRevision,
                    actualRevision,
                    streamId!);
        }

        void ValidateInput()
        {
            if (transient == null)
                throw new ArgumentNullException(nameof(transient));
            if (persistent == null)
                throw new ArgumentNullException(nameof(persistent));
            if (streamId == null)
                throw new ArgumentNullException(nameof(streamId));
        }
    }
}
