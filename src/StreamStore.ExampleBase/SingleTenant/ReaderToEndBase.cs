
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using StreamStore.ExampleBase.Progress;


namespace StreamStore.ExampleBase.SingleTenant
{
    [ExcludeFromCodeCoverage]
    public abstract class ReaderToEndBase : BackgroundService
    {
        readonly IStreamStore store;
        readonly ReadToEndProgressTracker progressTracker;
        const string streamId = "stream-1";

        protected ReaderToEndBase(IStreamStore store, ProgressTrackerFactory trackerFactory)
        {
            this.store = store;
            this.progressTracker = trackerFactory.SpawnReadToEndTracker(GetType().Name);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await new ReaderToEnd(store, streamId, progressTracker).BeginWorkAsync(3_000, stoppingToken);
        }
    }
}
