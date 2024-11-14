using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
            logger.LogInformation("Start reading stream");
            await foreach (var @event in await store.BeginReadAsync(streamId))
            {
                if (token.IsCancellationRequested) break;

                logger.LogInformation("Read event with id: {id}, revision: {revision}", @event.EventId, @event.Revision);
                logger.LogInformation("Waiting {sleepPeriod} seconds before next iteration.", sleepPeriod);

                await Task.Delay(sleepPeriod, token);
            }
        }
    }
}
