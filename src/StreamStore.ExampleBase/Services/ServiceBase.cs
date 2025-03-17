using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using StreamStore.ExampleBase.Progress;
using StreamStore.ExampleBase.Workers;

namespace StreamStore.ExampleBase.Services
{
    [ExcludeFromCodeCoverage]
    internal class ServiceBase : BackgroundService
    {
        protected readonly WorkerRegistry workerRegistry;
        readonly IStreamStore store;
        readonly Id tenant;

        readonly Id streamId = "stream-1";
        const int readerPerTenant = 2;
        const int writerPerTenant = 2;
        const int readerToEndPerTenant = 2;
        const int SleepPeriodDelta = 1_500;

        protected ServiceBase(
                   WorkerRegistry workerRegistry,
                   TenantContext context)
        {
            this.workerRegistry = workerRegistry;
            this.store = context.StreamStore;
            this.tenant = context.Tenant;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            List<Task> tasks = new List<Task>();

            tasks.AddRange(CreateTasks<Reader>(readerPerTenant, stoppingToken, CreateReaderIdentifier, RegisterReader));
            tasks.AddRange(CreateTasks<ReaderToEnd>(readerToEndPerTenant, stoppingToken, CreateReaderToEndIdentifier, RegisterReaderToEnd));
            tasks.AddRange(CreateTasks<Writer>(writerPerTenant, stoppingToken, CreateWriterIdentifier, RegisterWriter));

            Task.WaitAll(tasks.ToArray(), stoppingToken);
            return Task.CompletedTask;
        }


        void DoWork<TWorker>(int index, Func<int, WorkerIdentifier> createIdentifier, Action<TWorker> registerWorker, CancellationToken stoppingToken) where TWorker : WorkerBase
        {
            var identifier = createIdentifier(index);

            var worker = (TWorker)Activator.CreateInstance(typeof(TWorker), identifier, store, streamId);
            registerWorker(worker);
            worker.BeginWorkAsync(index * SleepPeriodDelta, stoppingToken).ConfigureAwait(false);
        }


        Task[] CreateTasks<TWorker>(int count, CancellationToken stoppingToken, Func<int, WorkerIdentifier> createIdentifier, Action<TWorker> registerWorker) where TWorker : WorkerBase
        {
            return Enumerable.Range(1, count).Select(i => Task.Run(() => DoWork(i, createIdentifier, registerWorker, stoppingToken))).ToArray();
        }

        void RegisterWriter(Writer writer)
        {
            workerRegistry.RegisterWriter(writer);

        }

        void RegisterReader(Reader reader)
        {
            workerRegistry.RegisterReader(reader);
        }


        void RegisterReaderToEnd(ReaderToEnd reader)
        {
            workerRegistry.RegisterReaderToEnd(reader);
        }

        WorkerIdentifier CreateReaderIdentifier(int index)
        {
            return new ReaderIdentifier(index, tenant);
        }
        WorkerIdentifier CreateWriterIdentifier(int index)
        {
            return new WriterIdentifier(index, tenant);
        }

        WorkerIdentifier CreateReaderToEndIdentifier(int index)
        {
            return new ReaderToEndIdentifier(index, tenant);
        }
    }
}
