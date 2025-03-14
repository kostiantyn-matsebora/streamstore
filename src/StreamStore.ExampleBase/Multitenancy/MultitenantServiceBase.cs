using Microsoft.Extensions.Logging;
using StreamStore.Multitenancy;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using Fare;
using System;
using StreamStore.ExampleBase.Progress;

namespace StreamStore.ExampleBase.Multitenancy
{
    [ExcludeFromCodeCoverage]
    internal abstract class MultitenantServiceBase<TWorker> : BackgroundService where TWorker : WorkerBase
    {
        protected readonly ProgressTrackerFactory trackerFactory;
        readonly ITenantStreamStoreFactory storeFactory;
        readonly ITenantProvider tenantProvider;
        readonly Id streamId = "stream-1";

        protected MultitenantServiceBase(
            ProgressTrackerFactory trackerFactory,
            ITenantStreamStoreFactory storeFactory,
            ITenantProvider tenantProvider)
        {
            this.trackerFactory = trackerFactory;
            this.storeFactory = storeFactory;
            this.tenantProvider = tenantProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var position = 0;

            var tasks = tenantProvider
                .GetAll()
                .Select(tenantId => Task.Run(() => DoWork(tenantId, ref position, stoppingToken)));

            Task.WaitAll(tasks.ToArray(), stoppingToken);
            return Task.CompletedTask;
        }

        void DoWork(Id tenantId, ref int position, CancellationToken stoppingToken)
        {
            var store = storeFactory.Create(tenantId);
            var worker = (TWorker)Activator.CreateInstance(typeof(TWorker), store!, streamId, CreateTracker(tenantId));
            worker.BeginWorkAsync(++position * SleepPeriodDelta, stoppingToken).ConfigureAwait(false);
        }


        protected abstract ProgressTracker CreateTracker(Id tenantId);


        protected abstract int SleepPeriodDelta { get; }
        protected abstract string Role { get; }

        protected string Identifier(Id tenantId) => $"{tenantId} {Role}".ToLowerInvariant();
    }
}
