using System.Diagnostics.CodeAnalysis;
using System.Threading;

using Microsoft.Extensions.Logging;
using StreamStore.ExampleBase.Progress;
using StreamStore.Multitenancy;

namespace StreamStore.ExampleBase.Multitenancy
{
    [ExcludeFromCodeCoverage]
    internal class MultitenantReader : MultitenantServiceBase<Reader>
    {

        public MultitenantReader(ProgressTrackerFactory trackerFactory, ITenantStreamStoreFactory storeFactory, ITenantProvider tenantProvider) :
            base(trackerFactory, storeFactory, tenantProvider)
        {
        }

        protected override string Role => nameof(Reader);

        protected override int SleepPeriodDelta => 2_000;

        protected override ProgressTracker CreateTracker(Id tenantId)
        {
            return trackerFactory.SpawnReadTracker(Identifier(tenantId));
        }
    }
}
