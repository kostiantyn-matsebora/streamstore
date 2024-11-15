namespace StreamStore.Testing.Serializer
{
    public abstract class SerializerScenario<TSuite> : Scenario<TSuite> where TSuite : SerializerSuiteBase, new()
    {
        protected IEventSerializer Serializer => Suite.Serializer;

        protected byte[] SerializedEvent => Suite.SerializedEvent;
        protected object DeserializedEvent => Suite.DeserializedEvent;

        protected SerializerScenario(TSuite suite) : base(suite)
        { }
    }
}
