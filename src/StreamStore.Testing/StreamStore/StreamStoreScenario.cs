namespace StreamStore.Testing.StreamStore
{
    public abstract class StreamStoreScenario<TEnvironment> : Scenario<TEnvironment> where TEnvironment : StreamStoreTestEnvironmentBase, new()
    {
        protected StreamStoreScenario(TEnvironment environment) : base(environment)
        {
        }

        protected IStreamStore Store => Environment.Store;

        protected MemoryDatabase Container => Environment.Container;
    }
}
