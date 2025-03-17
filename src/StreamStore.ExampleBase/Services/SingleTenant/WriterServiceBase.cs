using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using StreamStore.ExampleBase.Progress;
using StreamStore.ExampleBase.Workers;



namespace StreamStore.ExampleBase.Services.SingleTenant
{
    [ExcludeFromCodeCoverage]
    public abstract class WriterServiceBase : BackgroundService
    {
        readonly Writer writer;
        const string streamId = "stream-1";

        protected WriterServiceBase(IStreamStore store, WorkerRegistry workerFactory, int number)
        {
            writer = new Writer(new WriterIdentifier(number), store, streamId);
            workerFactory.RegisterWriter(writer);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await writer.BeginWorkAsync(100, stoppingToken);
        }
    }
}

