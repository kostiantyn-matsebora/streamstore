namespace StreamStore.Testing.Scenarios.Serializer
{
    public abstract class SerializerScenario<TSuite>: Scenario<TSuite> where TSuite : SerializerSuiteBase
    {
        protected IEventSerializer Serializer => Suite.Serializer;

        protected byte[] SerializedEvent => Suite.SerializedEvent;
        protected object DeserializedEvent => Suite.DeserializedEvent;

        protected SerializerScenario(TSuite suite) : base(suite)
        { }
    }
}
