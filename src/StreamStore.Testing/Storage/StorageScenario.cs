namespace StreamStore.Testing.StreamStorage
{
    public abstract class StorageScenario<TEnvironment> : Scenario<TEnvironment> where TEnvironment : StorageTestEnvironmentBase, new()
    {
        protected IStreamStorage Storage => Environment.StreamStorage;

        protected InMemoryStreamContainer Container => Environment.Container;

        protected StorageScenario(TEnvironment environment) : base(environment)
        {
        }
    }
}
