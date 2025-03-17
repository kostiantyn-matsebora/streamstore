using System.Diagnostics.CodeAnalysis;
using StreamStore.ExampleBase.Progress;


namespace StreamStore.ExampleBase.Services.Multitenancy
{
    [ExcludeFromCodeCoverage]
    internal class TenantOne : ServiceBase
    {
        public TenantOne(WorkerRegistry trackerFactory, TenantContextQueue contextQueue) : base(trackerFactory, contextQueue.Dequeue())
        {
        }
    }

    [ExcludeFromCodeCoverage]
    internal class TenantTwo : ServiceBase
    {
        public TenantTwo(WorkerRegistry trackerFactory, TenantContextQueue contextQueue) : base(trackerFactory, contextQueue.Dequeue())
        {
        }
    }

    [ExcludeFromCodeCoverage]
    internal class TenantThree : ServiceBase
    {
        public TenantThree(WorkerRegistry trackerFactory, TenantContextQueue contextQueue) : base(trackerFactory, contextQueue.Dequeue())
        {
        }
    }
}
