using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using StreamStore.ExampleBase.Progress;
using StreamStore.Multitenancy;

namespace StreamStore.ExampleBase.Multitenancy
{
    [ExcludeFromCodeCoverage]
    internal class TenantServiceBase : BackgroundService
    {
        protected readonly ProgressTrackerFactory trackerFactory;
        readonly ITenantStreamStoreFactory storeFactory;
        private readonly Id tenantId;

        readonly Id streamId = "stream-1";
        const int readerPerTenant = 2;
        const int writerPerTenant = 2;
        const int readerToEndPerTenant = 2;
        const int SleepPeriodDelta = 1_500;

        protected TenantServiceBase(
                   ProgressTrackerFactory trackerFactory,
                   ITenantStreamStoreFactory storeFactory,
                   TenantQueue tenantQueue)
        {
            this.trackerFactory = trackerFactory;
            this.storeFactory = storeFactory;
            this.tenantId = tenantQueue.DequeueTenant();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            List<Task> tasks = new List<Task>();

            tasks.AddRange(CreateTasks<Reader>(readerPerTenant, stoppingToken, CreateReadTracker));
            tasks.AddRange(CreateTasks<ReaderToEnd>(readerToEndPerTenant, stoppingToken, CreateReadToEndTracker));
            tasks.AddRange(CreateTasks<Writer>(writerPerTenant, stoppingToken, CreateWriteTracker));

            Task.WaitAll(tasks.ToArray(), stoppingToken);
            return Task.CompletedTask;
        }


        void DoWork<TWorker>(int position, ProgressTracker tracker, CancellationToken stoppingToken) where TWorker : WorkerBase
        {
            var store = storeFactory.Create(tenantId);
            var worker = (TWorker)Activator.CreateInstance(typeof(TWorker), store!, streamId, tracker);
            worker.BeginWorkAsync(position * SleepPeriodDelta, stoppingToken).ConfigureAwait(false);
        }


        Task[] CreateTasks<TWorker>(int count, CancellationToken stoppingToken, Func<int, ProgressTracker> trackerFactory) where TWorker : WorkerBase
        {
            return Enumerable.Range(1, count).Select(i => Task.Run(() => DoWork<TWorker>(i, trackerFactory(i), stoppingToken))).ToArray();
        }

        ProgressTracker CreateWriteTracker(int index)
        {
            return trackerFactory.SpawnWriteTracker(Identifier(tenantId, "writer", index));
        }

        ProgressTracker CreateReadTracker(int index)
        {
            return trackerFactory.SpawnReadTracker(Identifier(tenantId, "reader", index));
        }


        ProgressTracker CreateReadToEndTracker(int index)
        {
            return trackerFactory.SpawnReadToEndTracker(Identifier(tenantId, "readertoend", index));
        }


        string Identifier(Id tenantId, string role, int index) => $"{tenantId} {role}-{index}".ToLowerInvariant();
    }
}
