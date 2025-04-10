using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using StreamStore.Models;
using System.Linq;

namespace StreamStore
{
    static  class AsyncEnumerableExtension
    {
        public static async Task<IStreamEvent[]> ReadToEndAsync(this IAsyncEnumerable<IStreamEvent> enumerable, CancellationToken cancellationToken = default)
        {
            var results = new List<IStreamEvent>();

            await foreach (var item in enumerable)
            {
                results.Add(item);
            }
            return new StreamEventCollection(results).ToArray();
        }

        public static async Task<IStreamEvent[]> ReadToEndAsync(this Task<IAsyncEnumerable<IStreamEvent>> enumerable, CancellationToken cancellationToken = default)
        {
            return await FuncExtension.ThrowOriginalExceptionIfOccured(async () => await enumerable.GetAwaiter().GetResult().ReadToEndAsync(cancellationToken));
        }
    }
}
