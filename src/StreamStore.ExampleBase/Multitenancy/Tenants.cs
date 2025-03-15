using System.Diagnostics.CodeAnalysis;
using StreamStore.ExampleBase.Progress;
using StreamStore.Multitenancy;

namespace StreamStore.ExampleBase.Multitenancy
{
    [ExcludeFromCodeCoverage]
    internal class TenantOne : TenantServiceBase
    {
        public TenantOne(ProgressTrackerFactory trackerFactory, ITenantStreamStoreFactory storeFactory, TenantQueue tenantQueue) : base(trackerFactory, storeFactory, tenantQueue)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    internal class TenantTwo : TenantServiceBase
    {
        public TenantTwo(ProgressTrackerFactory trackerFactory, ITenantStreamStoreFactory storeFactory, TenantQueue tenantQueue) : base(trackerFactory, storeFactory, tenantQueue)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    internal class TenantThree : TenantServiceBase
    {
        public TenantThree(ProgressTrackerFactory trackerFactory, ITenantStreamStoreFactory storeFactory, TenantQueue tenantQueue) : base(trackerFactory, storeFactory, tenantQueue)
        {
        }
    }
}
