using System.Diagnostics.CodeAnalysis;
using System.Threading;

using Microsoft.Extensions.Logging;
using StreamStore.Multitenancy;

namespace StreamStore.ExampleBase.Multitenancy
{
    [ExcludeFromCodeCoverage]
    internal class MultitenantReader : MultitenantServiceBase<Reader>
    {

        public MultitenantReader(ILoggerFactory loggerFactory, ITenantStreamStoreFactory storeFactory, ITenantProvider tenantProvider):
            base(loggerFactory, storeFactory, tenantProvider)
        {
        }

        protected override string Role => nameof(Reader);

        protected override int SleepPeriodDelta => 2_000;
    }
}
