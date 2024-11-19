
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;


namespace StreamStore.ExampleBase.SingleTenant
{
    [ExcludeFromCodeCoverage]
    public abstract class ReaderToEndBase : BackgroundService
    {
        readonly ILogger logger;
        readonly IStreamStore store;
        const string streamId = "stream-1";

        protected ReaderToEndBase(ILogger logger, IStreamStore store)
        {
            this.logger = logger;
            this.store = store;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await new ReaderToEnd(logger, store, streamId).BeginWorkAsync(3_000, stoppingToken);
        }
    }
}
