using StreamStore.Testing.Framework;


namespace StreamStore.Serialization.Tests.TypeRegistry
{
    public class TypeRegistrySuite : TestSuiteBase
    {
        public Serialization.TypeRegistry TypeRegistry { get; }

        public TypeRegistrySuite()
        {
            TypeRegistry = new Serialization.TypeRegistry();
        }
    }
}
