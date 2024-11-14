using Microsoft.Extensions.Logging;
using StreamStore.Multitenancy;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace StreamStore.ExampleBase.Multitenancy
{
    [ExcludeFromCodeCoverage]
    internal class MultitenantReaderToEnd: MultitenantServiceBase<ReaderToEnd>
    {
        public MultitenantReaderToEnd(ILoggerFactory loggerFactory, ITenantStreamStoreFactory storeFactory, ITenantProvider tenantProvider) :
            base(loggerFactory, storeFactory, tenantProvider)
        {
        }

        protected override string Role => nameof(ReaderToEnd);

        protected override int SleepPeriodDelta => 1_000;
    }
}
