using System.Diagnostics.CodeAnalysis;

namespace StreamStore.ExampleBase.Services
{
    [ExcludeFromCodeCoverage]
    class TenantContext
    {
        public IStreamStore StreamStore { get; }
        public Id Tenant { get; }

        public TenantContext(IStreamStore store, Id tenant)
        {
            StreamStore = store;
            Tenant = tenant;
        }
    }
}
