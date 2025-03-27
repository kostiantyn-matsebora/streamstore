using StreamStore.Testing.Framework;


namespace StreamStore.Serialization.Tests.TypeRegistry
{
    public class TypeRegistryTestEnvironment : TestEnvironmentBase
    {
        public Serialization.TypeRegistry TypeRegistry { get; }

        public TypeRegistryTestEnvironment()
        {
            TypeRegistry = new Serialization.TypeRegistry();
        }
    }
}
