namespace StreamStore.Testing.StreamStore.Scenarios
{
    public abstract class Writing_to_stream<TSuite> : StreamStoreScenario<TSuite> where TSuite : StreamStoreSuiteBase, new()
    {
        protected Writing_to_stream(TSuite suite) : base(suite)
        {
        }
    }
}
