using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StopwatchTimer;

namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    internal class Reader: WorkerBase
    {
        public Reader(ILogger logger, IStreamStore store, Id streamId): base(logger, store, streamId)
        { }

        protected override int InitialSleepPeriod => 5_000;

        protected override async Task DoWorkAsync(int sleepPeriod, CancellationToken token)
        {
            logger.LogInformation("Start iterating stream");
            var enumerator = (await store.BeginReadAsync(streamId, token)).GetAsyncEnumerator();
            bool notEmpty = false;
            do
            {
                using (new CodeStopWatch("Taking next event", s => logger.LogInformation(s)))
                notEmpty = await enumerator.MoveNextAsync();
                Thread.Sleep(sleepPeriod);
            } while (notEmpty);
        }
    }
}
