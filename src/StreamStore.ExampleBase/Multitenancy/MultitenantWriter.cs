using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;
using StreamStore.Multitenancy;

namespace StreamStore.ExampleBase.Multitenancy
{
    [ExcludeFromCodeCoverage]
    internal class MultitenantWriter : MultitenantServiceBase<Writer>
    {
        public MultitenantWriter(ILoggerFactory loggerFactory, ITenantStreamStoreFactory storeFactory, ITenantProvider tenantProvider) :
            base(loggerFactory, storeFactory, tenantProvider)
        {
        }

        protected override string Role => nameof(Writer);

        protected override int SleepPeriodDelta => 1_500;
    }
}
