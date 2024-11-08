namespace StreamStore.Testing.StreamDatabase
{
    public abstract class DatabaseScenario<TSuite> : Scenario<TSuite> where TSuite : DatabaseSuiteBase, new()
    {
        protected IStreamDatabase Database => Suite.StreamDatabase;

        protected MemoryDatabase Container => Suite.Container;

        protected DatabaseScenario(TSuite suite) : base(suite)
        {
        }
    }
}
