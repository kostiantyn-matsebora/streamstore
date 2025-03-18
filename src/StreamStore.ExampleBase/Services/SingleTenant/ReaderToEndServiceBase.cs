
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using StreamStore.ExampleBase.Progress;
using StreamStore.ExampleBase.Workers;


namespace StreamStore.ExampleBase.Services.SingleTenant
{
    [ExcludeFromCodeCoverage]
    public abstract class ReaderToEndServiceBase : BackgroundService
    {
        readonly ReaderToEnd reader;
        const string streamId = "stream-1";

        protected ReaderToEndServiceBase(IStreamStore store, WorkerRegistry workerRegistry, int number)
        {
            reader = new ReaderToEnd(new ReaderToEndIdentifier(number), store, streamId);
            workerRegistry.RegisterReaderToEnd(reader);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await reader.BeginWorkAsync(3_000, stoppingToken);
        }
    }
}
