
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    public abstract  class ReaderBase : BackgroundService
    {
        readonly ILogger<ReaderBase> logger;
        readonly IStreamStore store;
        public const string StreamId = "stream-1";

        protected ReaderBase(ILogger<ReaderBase> logger, IStreamStore store)
        {
            this.logger = logger;
            this.store = store;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Waiting 5 seconds for storing at least something to database");
            Thread.Sleep(5000);
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Start reading stream");
                await foreach (var @event in await store.BeginReadAsync(StreamId))
                {
                    if (stoppingToken.IsCancellationRequested) break;

                    logger.LogInformation("Read event with id: {id}, revision: {revision}", @event.EventId, @event.Revision);
                    logger.LogInformation("Waiting 3 seconds before next iteration.");

                    await Task.Delay(3000, stoppingToken);
                }
            }
        }
    }
}
