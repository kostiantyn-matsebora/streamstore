using System.Diagnostics.CodeAnalysis;
using StreamStore.ExampleBase.Progress;
using StreamStore.Multitenancy;

namespace StreamStore.ExampleBase.Multitenancy
{
    [ExcludeFromCodeCoverage]
    internal class MultitenantWriter : MultitenantServiceBase<Writer>
    {
        public MultitenantWriter(ProgressTrackerFactory trackerFactory, ITenantStreamStoreFactory storeFactory, ITenantProvider tenantProvider) :
            base(trackerFactory, storeFactory, tenantProvider)
        {
        }

        protected override string Role => nameof(Writer);

        protected override int SleepPeriodDelta => 1_500;

        protected override ProgressTracker CreateTracker(Id tenantId)
        {
            return trackerFactory.SpawnWriteTracker(Identifier(tenantId));

        }
    }
}
