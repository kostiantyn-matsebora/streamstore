using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;

using System.Diagnostics.CodeAnalysis;
using StopwatchTimer;


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
            using (new CodeStopWatch("Reading stream to end", s => logger.LogInformation(s)))
            {
                await store.ReadToEndAsync(streamId, token);
            }
            logger.LogInformation("Sleeping for {sleepPeriod} miliseconds...", sleepPeriod);
            await Task.Delay(sleepPeriod, token);
        }
    }
}
