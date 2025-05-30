using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using StreamStore.Models;
using System.Linq;
using StreamStore.Extensions;

namespace StreamStore
{
    static  class AsyncEnumerableExtension
    {
        public static async Task<IStreamEventEnvelope[]> ReadToEndAsync(this IAsyncEnumerable<IStreamEventEnvelope> enumerable, CancellationToken cancellationToken = default)
        {
            var results = new List<IStreamEventEnvelope>();

            await foreach (var item in enumerable)
            {
                results.Add(item);
            }
            return new StreamEventCollection(results).ToArray();
        }

        public static async Task<IStreamEventEnvelope[]> ReadToEndAsync(this Task<IAsyncEnumerable<IStreamEventEnvelope>> enumerable, CancellationToken cancellationToken = default)
        {
            return await FuncExtension.ThrowOriginalExceptionIfOccured(async () => await enumerable.GetAwaiter().GetResult().ReadToEndAsync(cancellationToken));
        }
    }
}
