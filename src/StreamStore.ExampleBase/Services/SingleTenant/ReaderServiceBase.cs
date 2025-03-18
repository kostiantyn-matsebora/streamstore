using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using StreamStore.ExampleBase.Progress;
using StreamStore.ExampleBase.Workers;


namespace StreamStore.ExampleBase.Services.SingleTenant
{
    [ExcludeFromCodeCoverage]
    public abstract class ReaderServiceBase : BackgroundService
    {
        const string streamId = "stream-1";
        readonly Reader reader;

        protected ReaderServiceBase(IStreamStore store, WorkerRegistry workerRegistry, int number)
        {
            reader = new Reader(new ReaderIdentifier(number), store, streamId);
            workerRegistry.RegisterReader(reader);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await reader.BeginWorkAsync(5000, stoppingToken);
        }
    }
}
