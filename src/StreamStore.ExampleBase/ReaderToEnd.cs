using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Diagnostics.CodeAnalysis;


namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    internal class ReaderToEnd: WorkerBase
    {
        protected override int InitialSleepPeriod => 5_000;

        public ReaderToEnd(ILogger logger, IStreamStore store, Id streamId): base(logger, store, streamId)
        {
        }

        protected override async Task DoWorkAsync(int sleepPeriod, CancellationToken token)
        {
            var events = await store.ReadToEndAsync(streamId, token);
            logger.LogInformation("Read {count} events. Waiting {sleepPeriod} miliseconds before next iteration.", events.Count(), sleepPeriod);

            await Task.Delay(sleepPeriod, token);
        }
    }
}
