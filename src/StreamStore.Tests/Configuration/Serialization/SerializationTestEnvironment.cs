using StreamStore.Configuration;
using StreamStore.Serialization;
using StreamStore.Testing.Framework;

namespace StreamStore.Tests.Configuration.Serialization
{
    public class SerializationTestEnvironment: TestEnvironmentBase
    {

        public static ISerializationConfigurator CreateConfigurator() => new SerializationConfigurator();

        internal class FakeTypeRegistry : ITypeRegistry
        {
            public Type ResolveTypeByName(string name)
            {
                return typeof(string);
            }

            public string ResolveNameByType(Type type)
            {
                return type.Name;
            }
        }
    }
}
