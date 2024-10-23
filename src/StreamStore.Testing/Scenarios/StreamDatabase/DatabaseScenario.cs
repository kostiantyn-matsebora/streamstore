using StreamStore.Testing.Framework;

namespace StreamStore.Testing.Scenarios.StreamDatabase
{
    public abstract class DatabaseScenario<TSuite> : Scenario<TSuite> where TSuite : DatabaseSuiteBase
    {
        protected IStreamDatabase Database => Suite.StreamDatabase;

        protected MemoryDatabase Container => Suite.Container;

        protected DatabaseScenario(TSuite suite) : base(suite)
        {
        }
    }
}
