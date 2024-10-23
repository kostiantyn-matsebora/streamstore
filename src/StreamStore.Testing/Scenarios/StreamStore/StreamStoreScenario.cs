using System.Security.Cryptography;
using StreamStore.Testing.Framework;
using StreamStore.Testing.Models;


namespace StreamStore.Testing.Scenarios.StreamStore
{
    public abstract class StreamStoreScenario<TSuite> : Scenario<TSuite> where TSuite : StreamStoreSuiteBase
    {
        protected StreamStoreScenario(TSuite suite) : base(suite)
        {
        }

        protected IStreamStore Store => Suite.Store;

        protected MemoryDatabase Container => Suite.Container;
    }
}
