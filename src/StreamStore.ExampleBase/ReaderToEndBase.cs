
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    public abstract class ReaderToEndBase : BackgroundService
    {
        readonly ILogger<ReaderToEndBase> logger;
        readonly IStreamStore store;
        public const string StreamId = "stream-1";

        protected ReaderToEndBase(ILogger<ReaderToEndBase> logger, IStreamStore store)
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
                logger.LogInformation("Reading stream fully");

                var events = await store.ReadToEndAsync(StreamId, stoppingToken);
                logger.LogInformation("Read {count} events", events.Count());
                logger.LogInformation("Waiting 5 seconds before next iteration.");

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
