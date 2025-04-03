namespace StreamStore.Testing.StreamDatabase
{
    public abstract class DatabaseScenario<TEnvironment> : Scenario<TEnvironment> where TEnvironment : DatabaseTestEnvironmentBase, new()
    {
        protected IStreamStorage Database => Environment.StreamDatabase;

        protected MemoryDatabase Container => Environment.Container;

        protected DatabaseScenario(TEnvironment environment) : base(environment)
        {
        }
    }
}
