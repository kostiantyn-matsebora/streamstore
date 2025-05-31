using System.Diagnostics.CodeAnalysis;
using StreamStore.Extensions;
using StreamStore.Multitenancy;

namespace StreamStore.ExampleBase.Services.Multitenancy
{
    [ExcludeFromCodeCoverage]
    internal class TenantContextQueue
    {
        readonly ITenantStreamStoreFactory storeFactory;
        readonly TenantQueue queue;

        public TenantContextQueue(ITenantStreamStoreFactory storeFactory, TenantQueue queue) 
        {
            this.storeFactory = storeFactory.ThrowIfNull(nameof(storeFactory));
            this.queue = queue.ThrowIfNull(nameof(queue));
        }

        public TenantContext Dequeue()
        {
            var tenant = queue.DequeueTenant();
            return new TenantContext(storeFactory.Create(tenant), tenant);
        }

    }
}
