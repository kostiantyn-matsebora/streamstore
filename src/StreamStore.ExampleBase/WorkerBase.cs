using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    internal abstract class WorkerBase
    {
        protected readonly ILogger logger;
        protected readonly IStreamStore store;
        protected readonly Id streamId;

        protected WorkerBase(ILogger logger, IStreamStore store, Id streamId)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.store = store.ThrowIfNull(nameof(store));
            this.streamId = streamId.ThrowIfHasNoValue(nameof(streamId));
        }

        public async Task BeginWorkAsync(int sleepPeriod, CancellationToken token)
        {

            logger.LogInformation("Waiting {initialSleepPeriod} seconds for initialization...", InitialSleepPeriod);
            Thread.Sleep(InitialSleepPeriod);

            while (!token.IsCancellationRequested)
            {
                await DoWorkAsync(sleepPeriod, token);
            }
        }

        protected abstract int InitialSleepPeriod { get; }
        protected abstract Task DoWorkAsync(int sleepPeriod, CancellationToken token);
       
    }
}
