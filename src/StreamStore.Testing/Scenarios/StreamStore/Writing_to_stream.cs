namespace StreamStore.Testing.Scenarios.StreamStore
{
    public abstract class Writing_to_stream<TSuite> : StreamStoreScenario<TSuite> where TSuite : StreamStoreSuiteBase
    {
        protected Writing_to_stream(TSuite suite) : base(suite)
        {
        }
    }
}
