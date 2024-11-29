
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace StreamStore.ExampleBase.SingleTenant
{
    [ExcludeFromCodeCoverage]
    public abstract class ReaderBase : BackgroundService
    {
        readonly ILogger logger;
        readonly IStreamStore store;
        const string streamId = "stream-1";

        protected ReaderBase(ILogger logger, IStreamStore store)
        {
            this.logger = logger;
            this.store = store;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await new Reader(logger, store, streamId).BeginWorkAsync(2_000, stoppingToken);
        }
    }
}
