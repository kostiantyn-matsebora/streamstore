using StreamStore.Multitenancy;
using System.Diagnostics.CodeAnalysis;


namespace StreamStore.ExampleBase.Multitenancy
{
    [ExcludeFromCodeCoverage]
    internal class MultitenantReaderToEnd : MultitenantServiceBase<ReaderToEnd>
    {
        public MultitenantReaderToEnd(ProgressTrackerFactory trackerFactory, ITenantStreamStoreFactory storeFactory, ITenantProvider tenantProvider) :
            base(trackerFactory, storeFactory, tenantProvider)
        {
        }

        protected override string Role => nameof(ReaderToEnd);

        protected override int SleepPeriodDelta => 1_000;

        protected override ProgressTracker CreateTracker(Id tenantId)
        {
            return trackerFactory.SpawnReadToEndTracker(Identifier(tenantId));
        }
    }
}
