namespace StreamStore.Testing.Serializer
{
    public abstract class SerializerScenario<TEnvironment> : Scenario<TEnvironment> where TEnvironment : SerializerTestEnvironmentBase, new()
    {
        protected IEventSerializer Serializer => Environment.Serializer;

        protected byte[] SerializedEvent => Environment.SerializedEvent;
        protected object DeserializedEvent => Environment.DeserializedEvent;

        protected SerializerScenario(TEnvironment environment) : base(environment)
        { }
    }
}
