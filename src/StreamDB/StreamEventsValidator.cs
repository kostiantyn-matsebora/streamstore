using System.Collections.Generic;
using System.Linq;


namespace StreamDB
{
    internal class StreamEventsValidator<T1, T2>
        where T1 : IHasRevision, IHasId
        where T2 : IHasRevision, IHasId
    {
        public static void Validate(IEnumerable<T1> uncommited, IEnumerable<T2> commited, string streamId)
        {
            var uncommitedEvents = new StreamEvents<T1>(uncommited);
            var commitedEvents = new StreamEvents<T2>(commited);

            ThrowIfThereIsDuplicates(uncommitedEvents, commitedEvents, streamId);
            ThrowIfConccurrencyConflict(uncommitedEvents, commitedEvents, streamId);
        }

        static void ThrowIfThereIsDuplicates(StreamEvents<T1> uncommited, StreamEvents<T2> commited, string streamId)
        {
            var ids = uncommited.Select(e => e.Id).Concat(commited.Select(e => e.Id)).ToArray();
            var duplicates =
             ids.GroupBy(id => id)
                 .Where(g => g.Count() > 1)
                 .Select(g => g.Key)
                 .ToArray();

            if (duplicates.Any())
                throw new DuplicateEventException(duplicates, streamId);
        }

        static void ThrowIfConccurrencyConflict(StreamEvents<T1> uncommited, StreamEvents<T2> commited, Id streamId)
        {
            var expectedRevision = uncommited.MinRevision - 1;
            var actualRevision = commited.MaxRevision;

            if (expectedRevision != actualRevision)
                throw new OptimisticConcurrencyException(expectedRevision.GetValueOrDefault(), actualRevision.GetValueOrDefault(), streamId);
        }
    }
}
