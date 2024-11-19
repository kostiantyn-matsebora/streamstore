using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;



namespace StreamStore.ExampleBase.SingleTenant
{
    [ExcludeFromCodeCoverage]
    public abstract class WriterBase : BackgroundService
    {
        readonly ILogger logger;
        readonly IStreamStore store;
        const string streamId = "stream-1";

        protected WriterBase(ILogger logger, IStreamStore store)
        {
            this.logger = logger;
            this.store = store;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await new Writer(logger, store, streamId).BeginWorkAsync(3_000, stoppingToken);
        }
    }
}

