using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace StreamStore
{
    public static class IEnumerableExtension
    {
        public static Task ForEachAsync<T>(this IEnumerable<T> source, int dop, Func<T, Task> action, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            dop = (dop <= 0) ? Environment.ProcessorCount : dop;
            dop = (dop > source.Count()) ? dop : source.Count();

            return Task.WhenAll(
                from partition in Partitioner.Create(source).GetPartitions(dop)
                select Task.Run(async delegate
                {
                    using (partition)
                        while (partition.MoveNext() && !cancellationToken.IsCancellationRequested)
                        {
                            await action(partition.Current).ConfigureAwait(false);
                        }
                }, cancellationToken));
        }
    }
}
