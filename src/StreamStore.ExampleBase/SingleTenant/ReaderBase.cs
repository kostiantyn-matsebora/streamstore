using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;


namespace StreamStore.ExampleBase.SingleTenant
{
    [ExcludeFromCodeCoverage]
    public abstract class ReaderBase : BackgroundService
    {
        readonly IStreamStore store;
        const string streamId = "stream-1";
        readonly ReadProgressTracker progressTracker;

        protected ReaderBase(IStreamStore store, ProgressTrackerFactory trackerFactory)
        {
            this.progressTracker = trackerFactory.SpawnReadTracker(GetType().Name);
            this.store = store;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await new Reader(store, streamId, progressTracker).BeginWorkAsync(5000, stoppingToken);
        }
    }
}
